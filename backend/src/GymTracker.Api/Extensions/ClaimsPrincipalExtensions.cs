using GymTracker.Application.Exceptions;
using System.Security.Claims;

namespace GymTracker.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var sub = user.FindFirst("sub")?.Value
            ?? throw new UnauthorizedException("Invalid token.");
        return int.Parse(sub);
    }
}