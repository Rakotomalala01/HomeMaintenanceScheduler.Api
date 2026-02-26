using HomeMaintenanceScheduler.Api.Entities;

namespace HomeMaintenanceScheduler.Api.Models;

public class TaskResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Category { get; set; }

    public RecurrenceType RecurrenceType { get; set; }
    public int RecurrenceInterval { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly NextDueDate { get; set; }

    public bool IsActive { get; set; }
}