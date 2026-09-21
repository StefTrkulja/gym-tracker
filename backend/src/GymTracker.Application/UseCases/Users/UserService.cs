using BCrypt.Net;
using GymTracker.Application.Contracts.Persistence;
using GymTracker.Application.Contracts.UseCases.Users;
using GymTracker.Application.DTOs.Users;
using GymTracker.Domain.Models;

namespace GymTracker.Application.UseCases.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    

    public async Task<UserResponse> UpdateAsync(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user is null) throw new KeyNotFoundException($"User with id {request.Id} not found.");

        if (user.Email != request.Email &&
            await _userRepository.GetByEmailAsync(request.Email, cancellationToken) is not null)
            throw new InvalidOperationException("A user with this email already exists.");

        if (user.Username != request.Username &&
            await _userRepository.GetByUsernameAsync(request.Username, cancellationToken) is not null)
            throw new InvalidOperationException("A user with this username already exists.");

        user.Username = request.Username;
        user.Email = request.Email;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        var updated = await _userRepository.Update(user, cancellationToken);
        return new UserResponse(updated.Id, updated.Username, updated.Email, updated.FirstName, updated.LastName);
    }

    public async Task<UserResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user is null) return null;
        return new UserResponse(user.Id, user.Username, user.Email, user.FirstName, user.LastName);
    }

    public async Task<UserResponse?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
        if (user is null) return null;
        return new UserResponse(user.Id, user.Username, user.Email, user.FirstName, user.LastName);
    }

}