using GymTracker.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymTracker.Application.Contracts.Persistence
{
    public interface IWorkoutRepository
    {
        Task<Workout> CreateAsync(Workout workout, CancellationToken cancellationToken);
        Task<Workout> UpdateAsync(Workout workout, CancellationToken cancellationToken);
        Task DeleteAsync(Workout workout, CancellationToken cancellationToken);
        Task<Workout?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<Workout>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken);

        Task<List<Workout>> GetByUserIdAndDateRangeAsync(int userId, DateTime from, DateTime to, CancellationToken cancellationToken);
    }
}
