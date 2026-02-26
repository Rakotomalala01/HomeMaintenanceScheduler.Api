using HomeMaintenanceScheduler.Api.Entities;

namespace HomeMaintenanceScheduler.Api.Models;

public class CreateTaskRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Category { get; set; }

    public RecurrenceType RecurrenceType { get; set; }
    public int RecurrenceInterval { get; set; }

    public DateOnly StartDate { get; set; }

    // Optional: if omitted, we default NextDueDate = StartDate
    public DateOnly? NextDueDate { get; set; }

    public bool IsActive { get; set; } = true;
}