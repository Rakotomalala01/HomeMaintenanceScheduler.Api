using HomeMaintenanceScheduler.Api.Services;

namespace HomeMaintenanceScheduler.Api.Background;

public class ReminderWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ReminderWorker> _logger;

    public ReminderWorker(
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        ILogger<ReminderWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var intervalMinutes = _configuration.GetValue<int?>("ReminderSettings:WorkerIntervalMinutes") ?? 60;

        if (intervalMinutes <= 0)
        {
            _logger.LogWarning("Reminder worker interval is invalid. Falling back to 60 minutes.");
            intervalMinutes = 60;
        }

        _logger.LogInformation("ReminderWorker started. Running every {IntervalMinutes} minute(s).", intervalMinutes);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var reminderScanService = scope.ServiceProvider.GetRequiredService<ReminderScanService>();

                var result = await reminderScanService.RunAsync(stoppingToken);

                _logger.LogInformation(
                    "Reminder scan completed. DueSoonSent={DueSoonSent}, OverdueSent={OverdueSent}, SkippedDuplicates={SkippedDuplicates}",
                    result.DueSoonSent,
                    result.OverdueSent,
                    result.SkippedDuplicates);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("ReminderWorker cancellation requested.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ReminderWorker failed during reminder scan.");
            }

            try
            {
                await Task.Delay(TimeSpan.FromMinutes(intervalMinutes), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("ReminderWorker delay cancelled.");
                break;
            }
        }

        _logger.LogInformation("ReminderWorker stopped.");
    }
}