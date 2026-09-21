using System.ComponentModel.DataAnnotations;

namespace GymTracker.Application.DTOs.Auth;

public record LoginRequest(
    [Required][EmailAddress][MaxLength(50)] string Email,
    [Required][MaxLength(50)] string Password
);