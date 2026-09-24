using GymTracker.Api.Extensions;
using GymTracker.Application.Contracts.UseCases.BodyMeasurements;
using GymTracker.Application.DTOs.BodyMeasurements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BodyMeasurementsController : ControllerBase
    {
        private readonly IBodyMeasurementService _bodyMeasurementService;

        public BodyMeasurementsController(IBodyMeasurementService bodyMeasurementService)
        {
            _bodyMeasurementService = bodyMeasurementService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var measurements = await _bodyMeasurementService.GetAllByUserIdAsync(userId, cancellationToken);
            return Ok(measurements);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBodyMeasurementRequest request, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var created = await _bodyMeasurementService.CreateAsync(userId, request, cancellationToken);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBodyMeasurementRequest request, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var updated = await _bodyMeasurementService.UpdateAsync(userId, id, request, cancellationToken);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            await _bodyMeasurementService.DeleteAsync(userId, id, cancellationToken);
            return NoContent();
        }
    }
}
