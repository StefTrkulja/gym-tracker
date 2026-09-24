using GymTracker.Application.DTOs.Auth;
using GymTracker.Application.DTOs.Users;

namespace GymTracker.Application.Contracts.UseCases.Auth;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<LoginResponse> RegisterAsync(CreateUserRequest request, CancellationToken cancellationToken);
    Task<LoginResponse> GoogleLoginAsync(string idToken, CancellationToken cancellationToken);
}

