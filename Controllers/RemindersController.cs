using HomeMaintenanceScheduler.Api.Models;
using HomeMaintenanceScheduler.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeMaintenanceScheduler.Api.Controllers;

[ApiController]
[Route("reminders")]
public class RemindersController : ControllerBase
{
    private readonly ReminderScanService _reminderScanService;

    public RemindersController(ReminderScanService reminderScanService)
    {
        _reminderScanService = reminderScanService;
    }

    [HttpPost("run")]
    public async Task<ActionResult<ReminderRunResponse>> Run(CancellationToken cancellationToken)
    {
        var result = await _reminderScanService.RunAsync(cancellationToken);
        return Ok(result);
    }
}