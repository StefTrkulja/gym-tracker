using Microsoft.EntityFrameworkCore;
using GymTracker.Application.Contracts.Persistence;
using GymTracker.Domain.Models;

namespace GymTracker.Infrastructure.Persistence.Workouts;

public class WorkoutRepository : IWorkoutRepository
{
    private readonly DataContext _context;

    public WorkoutRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Workout> CreateAsync(Workout workout, CancellationToken cancellationToken)
    {
        _context.Workouts.Add(workout);
        await _context.SaveChangesAsync(cancellationToken);
        return workout;
    }

    public async Task<Workout> UpdateAsync(Workout workout, CancellationToken cancellationToken)
    {
        _context.Workouts.Update(workout);
        await _context.SaveChangesAsync(cancellationToken);
        return workout;
    }

    public async Task DeleteAsync(Workout workout, CancellationToken cancellationToken)
    {
        _context.Workouts.Remove(workout);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Workout?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Workouts.FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    public async Task<List<Workout>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        return await _context.Workouts
            .Where(w => w.UserId == userId)
            .OrderByDescending(w => w.PerformedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Workout>> GetByUserIdAndDateRangeAsync(int userId, DateTime from, DateTime to, CancellationToken cancellationToken)
    {
        return await _context.Workouts
            .Where(w => w.UserId == userId && w.PerformedAt >= from && w.PerformedAt <= to)
            .OrderBy(w => w.PerformedAt)
            .ToListAsync(cancellationToken);
    }
}