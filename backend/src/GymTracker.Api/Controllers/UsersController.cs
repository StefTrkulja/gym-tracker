using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GymTracker.Application.Contracts.UseCases.Users;
using GymTracker.Application.DTOs.Users;
using System.IdentityModel.Tokens.Jwt;

namespace GymTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }


    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var currentUserId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        if (currentUserId != request.Id)
            return Forbid();

        var user = await _userService.UpdateAsync(request, cancellationToken);
        return Ok(user);
    }
}
