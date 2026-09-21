using GymTracker.Domain.Models;

namespace GymTracker.Application.Contracts.Infrastructure;

public interface ITokenService
{
    string GenerateToken(User user);
}

