using HomeMaintenanceScheduler.Api.Entities;

namespace HomeMaintenanceScheduler.Api.Services;

public class RecurrenceService
{
    public DateOnly CalculateNextDueDate(DateOnly baseDate, RecurrenceType type, int interval)
    {
        if (interval <= 0)
            throw new ArgumentOutOfRangeException(nameof(interval), "RecurrenceInterval must be > 0");

        return type switch
        {
            RecurrenceType.Days => baseDate.AddDays(interval),
            RecurrenceType.Weeks => baseDate.AddDays(7 * interval),
            RecurrenceType.Months => AddMonthsSafe(baseDate, interval),
            _ => throw new ArgumentOutOfRangeException(nameof(type), $"Unsupported recurrence type: {type}")
        };
    }

    // Handles end-of-month cases safely (ex: Jan 31 + 1 month => Feb 28/29)
    private static DateOnly AddMonthsSafe(DateOnly date, int months)
    {
        var dt = date.ToDateTime(TimeOnly.MinValue);
        var next = dt.AddMonths(months);
        return DateOnly.FromDateTime(next);
    }
}