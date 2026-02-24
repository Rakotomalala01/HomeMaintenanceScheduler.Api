namespace HomeMaintenanceScheduler.Api.Entities;

public class ReminderLog
{
    public int Id { get; set; }

    public int TaskId { get; set; }
    public MaintenanceTask Task { get; set; } = null!;

    public DateOnly DueDate { get; set; }

    public ReminderType Type { get; set; }

    public DateTime SentAt { get; set; } // Always store as UTC
    public string Channel { get; set; } = "Discord";
}