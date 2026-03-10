namespace HomeMaintenanceScheduler.Api.Models;

public class CompleteTaskRequest
{
    public DateOnly? CompletedAt { get; set; }
    public string? Note { get; set; }
}