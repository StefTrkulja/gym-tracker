using GymTracker.Application.Contracts.Persistence;
using GymTracker.Application.Contracts.UseCases.Workouts;
using GymTracker.Application.DTOs.Workouts;
using GymTracker.Application.Exceptions;
using GymTracker.Domain.Models;

namespace GymTracker.Application.UseCases.Workouts;

public class WorkoutService : IWorkoutService
{
    private readonly IWorkoutRepository _workoutRepository;

    public WorkoutService(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }

    public async Task<WorkoutResponse> CreateAsync(int userId, CreateWorkoutRequest request, CancellationToken cancellationToken)
    {
        var workout = new Workout(userId, request.ExerciseType, request.DurationMinutes, request.CaloriesBurned, request.Intensity, request.Fatigue, request.Notes, request.PerformedAt);

        var created = await _workoutRepository.CreateAsync(workout, cancellationToken);
        return ToResponse(created);
    }

    public async Task<WorkoutResponse> UpdateAsync(int userId, int workoutId, UpdateWorkoutRequest request, CancellationToken cancellationToken)
    {
        var workout = await _workoutRepository.GetByIdAsync(workoutId, cancellationToken);
        if (workout is null)
            throw new NotFoundException($"Workout with id {workoutId} not found.");

        if (!workout.IsOwnedBy(userId))
            throw new ForbiddenException("You are not allowed to modify this workout.");

        workout.Update(request.ExerciseType, request.DurationMinutes, request.CaloriesBurned, request.Intensity, request.Fatigue, request.Notes, request.PerformedAt);

        var updated = await _workoutRepository.UpdateAsync(workout, cancellationToken);
        return ToResponse(updated);
    }

    public async Task DeleteAsync(int userId, int workoutId, CancellationToken cancellationToken)
    {
        var workout = await _workoutRepository.GetByIdAsync(workoutId, cancellationToken);
        if (workout is null)
            throw new NotFoundException($"Workout with id {workoutId} not found.");

        if (workout.UserId != userId)
            throw new ForbiddenException("You are not allowed to delete this workout.");

        await _workoutRepository.DeleteAsync(workout, cancellationToken);
    }

    public async Task<List<WorkoutResponse>> GetAllForUserAsync(int userId, CancellationToken cancellationToken)
    {
        var workouts = await _workoutRepository.GetAllByUserIdAsync(userId, cancellationToken);
        return workouts.Select(ToResponse).ToList();
    }

    private static WorkoutResponse ToResponse(Workout workout) => new(
        workout.Id,
        workout.ExerciseType,
        workout.DurationMinutes,
        workout.CaloriesBurned,
        workout.Intensity,
        workout.Fatigue,
        workout.Notes,
        workout.PerformedAt
    );
}