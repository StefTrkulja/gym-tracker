using GymTracker.Application.Contracts.Infrastructure;
using GymTracker.Application.Contracts.Persistence;
using GymTracker.Application.Contracts.UseCases.Auth;
using GymTracker.Application.DTOs.Auth;
using GymTracker.Application.DTOs.Users;
using GymTracker.Domain.Models;

namespace GymTracker.Application.UseCases.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> RegisterAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var existingEmail = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        var existingUsername = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);

        if (existingEmail is not null)
            throw new InvalidOperationException("A user with this email already exists.");

        if (existingUsername is not null)
            throw new InvalidOperationException("A user with this username already exists.");


        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHashed = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName

        };

        var created = await _userRepository.CreateAsync(user, cancellationToken);
        
        var token = _tokenService.GenerateToken(created);

        return new LoginResponse(token);

    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHashed))
            throw new UnauthorizedAccessException("Invalid email or password.");

        var token = _tokenService.GenerateToken(user);
        return new LoginResponse(token);
    }
}