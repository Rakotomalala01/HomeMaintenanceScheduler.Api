namespace HomeMaintenanceScheduler.Api.Entities;

public class MaintenanceTask
{
    public int Id { get; set; }

    // Basic info
    public string Title { get; set; } = string.Empty;
    public string? Category { get; set; }

    // Recurrence configuration
    public RecurrenceType RecurrenceType { get; set; }
    public int RecurrenceInterval { get; set; }

    // Dates
    public DateOnly StartDate { get; set; }
    public DateOnly NextDueDate { get; set; }

    // Status
    public bool IsActive { get; set; } = true;

    // Navigation property
    public ICollection<TaskCompletion> Completions { get; set; } = new List<TaskCompletion>();
}