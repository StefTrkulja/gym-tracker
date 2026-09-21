using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GymTracker.Application.Contracts.UseCases.Workouts;
using GymTracker.Application.DTOs.Workouts;

namespace GymTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WorkoutsController : ControllerBase
{
    private readonly IWorkoutService _workoutService;

    public WorkoutsController(IWorkoutService workoutService)
    {
        _workoutService = workoutService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var workouts = await _workoutService.GetAllForUserAsync(userId, cancellationToken);
        return Ok(workouts);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWorkoutRequest request, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var workout = await _workoutService.CreateAsync(userId, request, cancellationToken);
        return Ok(workout);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateWorkoutRequest request, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var workout = await _workoutService.UpdateAsync(userId, id, request, cancellationToken);
        return Ok(workout);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        await _workoutService.DeleteAsync(userId, id, cancellationToken);
        return NoContent();
    }

    private int GetUserId()
    {
        var sub = User.FindFirst("sub")?.Value
            ?? throw new UnauthorizedAccessException("Invalid token.");
        return int.Parse(sub);
    }
}