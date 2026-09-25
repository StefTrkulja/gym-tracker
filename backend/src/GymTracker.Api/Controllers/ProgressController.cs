using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GymTracker.Application.Contracts.UseCases.Progress;
using GymTracker.Api.Extensions;

namespace GymTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProgressController : ControllerBase
{
    private readonly IProgressService _progressService;

    public ProgressController(IProgressService progressService)
    {
        _progressService = progressService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMonthlyProgress([FromQuery] int year, [FromQuery] int month, CancellationToken cancellationToken)
    {
       
        var userId = User.GetUserId();
        var progress = await _progressService.GetMonthlyProgressAsync(userId, year, month, cancellationToken);
        return Ok(progress);
    }
}