using Google.Apis.Auth;
using GymTracker.Application.Contracts.Infrastructure;
using GymTracker.Application.Contracts.Persistence;
using GymTracker.Application.Contracts.UseCases.Auth;
using GymTracker.Application.DTOs.Auth;
using GymTracker.Application.DTOs.Users;
using GymTracker.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace GymTracker.Application.UseCases.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;


    public AuthService(IUserRepository userRepository, ITokenService tokenService, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _configuration = configuration;
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

    public async Task<LoginResponse> GoogleLoginAsync(string idToken, CancellationToken cancellationToken)
    {
        GoogleJsonWebSignature.Payload payload;
        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _configuration["GoogleAuth:ClientId"] }
            });
        }
        catch (InvalidJwtException)
        {
            throw new UnauthorizedAccessException("Invalid Google token.");
        }

        var user = await _userRepository.GetByEmailAsync(payload.Email, cancellationToken);

        if (user is null)
        {
            var username = await GenerateUniqueUsernameAsync(payload.Email, cancellationToken);

            user = new User
            {
                Username = username,
                Email = payload.Email,
                FirstName = payload.GivenName ?? "",
                LastName = payload.FamilyName ?? "",
                PasswordHashed = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()),
                IsGoogleAccount = true
            };
            user = await _userRepository.CreateAsync(user, cancellationToken);
        }

        var token = _tokenService.GenerateToken(user);
        return new LoginResponse(token);
    }
    private async Task<string> GenerateUniqueUsernameAsync(string email, CancellationToken cancellationToken)
    {
        var baseUsername = email.Split('@')[0];
        var username = baseUsername;
        var suffix = 1;

        while (await _userRepository.GetByUsernameAsync(username, cancellationToken) is not null)
        {
            username = $"{baseUsername}{suffix}";
            suffix++;
        }

        return username;
    }

}