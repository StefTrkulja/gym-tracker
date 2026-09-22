using GymTracker.Application.DTOs.Workouts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymTracker.Application.Contracts.UseCases.Workouts
{
    public interface IWorkoutService
    {
        Task<WorkoutResponse> CreateAsync(int userId, CreateWorkoutRequest request, CancellationToken cancellationToken);
        Task<WorkoutResponse> UpdateAsync(int userId, int workoutId, UpdateWorkoutRequest request, CancellationToken cancellationToken);
        Task DeleteAsync(int userId, int workoutId, CancellationToken cancellationToken);
        Task<List<WorkoutResponse>> GetAllForUserAsync(int userId, CancellationToken cancellationToken);
    }
}
