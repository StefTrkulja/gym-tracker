using GymTracker.Application.DTOs.Users;

namespace GymTracker.Application.Contracts.UseCases.Users;

public interface IUserService
{
    Task<UserResponse> UpdateAsync(int userId,UpdateUserRequest request, CancellationToken cancellationToken);
    Task<UserResponse> GetByIdAsync(int id, CancellationToken cancellationToken);
}
    