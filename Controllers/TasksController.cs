using HomeMaintenanceScheduler.Api.Data;
using HomeMaintenanceScheduler.Api.Entities;
using HomeMaintenanceScheduler.Api.Models;
using HomeMaintenanceScheduler.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeMaintenanceScheduler.Api.Controllers;

[ApiController]
[Route("tasks")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly RecurrenceService _recurrence;

    public TasksController(AppDbContext db, RecurrenceService recurrence)
    {
        _db = db;
        _recurrence = recurrence;
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponse>> Create([FromBody] CreateTaskRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest("Title is required.");

        if (request.RecurrenceInterval <= 0)
            return BadRequest("RecurrenceInterval must be > 0.");

        var start = request.StartDate;
        var nextDue = request.NextDueDate ?? start;

        var task = new MaintenanceTask
        {
            Title = request.Title.Trim(),
            Category = request.Category,
            RecurrenceType = request.RecurrenceType,
            RecurrenceInterval = request.RecurrenceInterval,
            StartDate = start,
            NextDueDate = nextDue,
            IsActive = request.IsActive
        };

        _db.MaintenanceTasks.Add(task);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = task.Id }, ToResponse(task));
    }

    [HttpGet]
    public async Task<ActionResult<List<TaskResponse>>> GetAll()
    {
        var tasks = await _db.MaintenanceTasks
            .OrderBy(t => t.NextDueDate)
            .Select(t => ToResponse(t))
            .ToListAsync();

        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponse>> GetById([FromRoute] int id)
    {
        var task = await _db.MaintenanceTasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task is null)
            return NotFound();

        return Ok(ToResponse(task));
    }

    [HttpPost("{id:int}/complete")]
    public async Task<ActionResult<TaskResponse>> Complete([FromRoute] int id, [FromBody] CompleteTaskRequest request)
    {
        var task = await _db.MaintenanceTasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task is null)
            return NotFound();

        if (!task.IsActive)
            return BadRequest("Task is inactive and cannot be completed.");

        var completedAt = request.CompletedAt ?? DateOnly.FromDateTime(DateTime.UtcNow);

        var completion = new TaskCompletion
        {
            TaskId = task.Id,
            CompletedAt = completedAt,
            Note = request.Note
        };

        _db.TaskCompletions.Add(completion);

        task.NextDueDate = _recurrence.CalculateNextDueDate(
            completedAt,
            task.RecurrenceType,
            task.RecurrenceInterval);

        await _db.SaveChangesAsync();

        return Ok(ToResponse(task));
    }

    [HttpGet("{id:int}/history")]
    public async Task<ActionResult<List<TaskCompletionResponse>>> GetHistory([FromRoute] int id)
    {
        var taskExists = await _db.MaintenanceTasks.AnyAsync(t => t.Id == id);
        if (!taskExists)
            return NotFound();

        var history = await _db.TaskCompletions
            .Where(tc => tc.TaskId == id)
            .OrderByDescending(tc => tc.CompletedAt)
            .Select(tc => new TaskCompletionResponse
            {
                Id = tc.Id,
                TaskId = tc.TaskId,
                CompletedAt = tc.CompletedAt,
                Note = tc.Note
            })
            .ToListAsync();

        return Ok(history);
    }

    private static TaskResponse ToResponse(MaintenanceTask t) => new()
    {
        Id = t.Id,
        Title = t.Title,
        Category = t.Category,
        RecurrenceType = t.RecurrenceType,
        RecurrenceInterval = t.RecurrenceInterval,
        StartDate = t.StartDate,
        NextDueDate = t.NextDueDate,
        IsActive = t.IsActive
    };
}