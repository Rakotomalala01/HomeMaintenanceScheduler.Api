namespace HomeMaintenanceScheduler.Api.Models;

public class ReminderRunResponse
{
    public int DueSoonSent { get; set; }
    public int OverdueSent { get; set; }
    public int SkippedDuplicates { get; set; }
}