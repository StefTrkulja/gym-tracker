using GymTracker.Domain.Models;

namespace GymTracker.Application.Contracts.Persistence;

public interface IUserRepository
{
    
    public Task<User> CreateAsync(User user, CancellationToken cancellationToken);
    public Task<User> Update(User user, CancellationToken cancellationToken);
    public Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    
}