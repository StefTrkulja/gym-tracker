using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GymTracker.Application.Contracts.UseCases.Users;
using GymTracker.Application.DTOs.Users;
using System.IdentityModel.Tokens.Jwt;
using GymTracker.Api.Extensions;

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
        var userId = User.GetUserId();

        var user = await _userService.UpdateAsync(userId,request, cancellationToken);
        return Ok(user);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var user = await _userService.GetByIdAsync(userId, cancellationToken);

        return Ok(user);
    
    }
}

