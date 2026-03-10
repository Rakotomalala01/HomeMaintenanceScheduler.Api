namespace HomeMaintenanceScheduler.Api.Models;

public class TaskCompletionResponse
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public DateOnly CompletedAt { get; set; }
    public string? Note { get; set; }
}