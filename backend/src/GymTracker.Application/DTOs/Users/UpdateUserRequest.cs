using System.ComponentModel.DataAnnotations;

namespace GymTracker.Application.DTOs.Users;

public record UpdateUserRequest(
    [Required][MaxLength(50)]
    [RegularExpression(@"^[a-zA-Z0-9_.]{3,50}$", ErrorMessage = "Username must be 3-50 characters and contain only letters, numbers, '_' or '.'.")] string Username,
    [Required][EmailAddress][MaxLength(255)] string Email,
     [Required][MaxLength(50)] string FirstName,
    [Required][MaxLength(50)] string LastName
);
 