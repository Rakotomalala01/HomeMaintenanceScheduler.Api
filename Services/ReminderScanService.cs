using HomeMaintenanceScheduler.Api.Data;
using HomeMaintenanceScheduler.Api.Entities;
using HomeMaintenanceScheduler.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeMaintenanceScheduler.Api.Services;

public class ReminderScanService
{
    private readonly AppDbContext _db;
    private readonly DiscordNotifier _discordNotifier;
    private readonly IConfiguration _configuration;

    public ReminderScanService(
        AppDbContext db,
        DiscordNotifier discordNotifier,
        IConfiguration configuration)
    {
        _db = db;
        _discordNotifier = discordNotifier;
        _configuration = configuration;
    }

    public async Task<ReminderRunResponse> RunAsync(CancellationToken cancellationToken = default)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var dueSoonDays = _configuration.GetValue<int?>("ReminderSettings:DueSoonDays") ?? 7;
        var dueSoonEnd = today.AddDays(dueSoonDays);

        var tasks = await _db.MaintenanceTasks
            .Where(t => t.IsActive)
            .OrderBy(t => t.NextDueDate)
            .ToListAsync(cancellationToken);

        var result = new ReminderRunResponse();

        foreach (var task in tasks)
        {
            if (task.NextDueDate < today)
            {
                var alreadySentToday = await _db.ReminderLogs.AnyAsync(r =>
                    r.TaskId == task.Id &&
                    r.Type == ReminderType.Overdue &&
                    r.SentAt.Date == DateTime.UtcNow.Date,
                    cancellationToken);

                if (alreadySentToday)
                {
                    result.SkippedDuplicates++;
                    continue;
                }

                var daysOverdue = today.DayNumber - task.NextDueDate.DayNumber;
                var message = $"🚨 Overdue: {task.Title} (Due: {task.NextDueDate:yyyy-MM-dd}) - {daysOverdue} days overdue";

                await _discordNotifier.SendMessageAsync(message, cancellationToken);

                _db.ReminderLogs.Add(new ReminderLog
                {
                    TaskId = task.Id,
                    DueDate = task.NextDueDate,
                    Type = ReminderType.Overdue,
                    SentAt = DateTime.UtcNow,
                    Channel = "Discord"
                });

                result.OverdueSent++;
            }
            else if (task.NextDueDate >= today && task.NextDueDate <= dueSoonEnd)
            {
                var alreadySent = await _db.ReminderLogs.AnyAsync(r =>
                    r.TaskId == task.Id &&
                    r.DueDate == task.NextDueDate &&
                    r.Type == ReminderType.DueSoon,
                    cancellationToken);

                if (alreadySent)
                {
                    result.SkippedDuplicates++;
                    continue;
                }

                var message = $"🔧 Due soon: {task.Title} (Due: {task.NextDueDate:yyyy-MM-dd})";

                await _discordNotifier.SendMessageAsync(message, cancellationToken);

                _db.ReminderLogs.Add(new ReminderLog
                {
                    TaskId = task.Id,
                    DueDate = task.NextDueDate,
                    Type = ReminderType.DueSoon,
                    SentAt = DateTime.UtcNow,
                    Channel = "Discord"
                });

                result.DueSoonSent++;
            }
        }

        await _db.SaveChangesAsync(cancellationToken);

        return result;
    }
}