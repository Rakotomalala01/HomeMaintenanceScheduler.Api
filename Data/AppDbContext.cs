using HomeMaintenanceScheduler.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeMaintenanceScheduler.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<MaintenanceTask> MaintenanceTasks => Set<MaintenanceTask>();
    public DbSet<TaskCompletion> TaskCompletions => Set<TaskCompletion>();
    public DbSet<ReminderLog> ReminderLogs => Set<ReminderLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MaintenanceTask>()
            .Property(x => x.StartDate)
            .HasConversion<DateOnlyConverter>();

        modelBuilder.Entity<MaintenanceTask>()
            .Property(x => x.NextDueDate)
            .HasConversion<DateOnlyConverter>();

        modelBuilder.Entity<TaskCompletion>()
            .Property(x => x.CompletedAt)
            .HasConversion<DateOnlyConverter>();

        modelBuilder.Entity<ReminderLog>()
            .Property(x => x.DueDate)
            .HasConversion<DateOnlyConverter>();
    }
}