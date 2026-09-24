using System.ComponentModel.DataAnnotations;

namespace GymTracker.Application.DTOs.Auth;

public record LoginRequest(
    [Required][EmailAddress][MaxLength(255)] string Email,
    [Required][MaxLength(72)] string Password
);