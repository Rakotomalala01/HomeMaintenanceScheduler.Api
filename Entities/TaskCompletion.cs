namespace HomeMaintenanceScheduler.Api.Entities;

public class TaskCompletion
{
    public int Id { get; set; }

    public int TaskId { get; set; }
    public MaintenanceTask Task { get; set; } = null!;

    public DateOnly CompletedAt { get; set; }
    public string? Note { get; set; }
}