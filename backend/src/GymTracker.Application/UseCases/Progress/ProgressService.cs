using GymTracker.Application.Contracts.Persistence;
using GymTracker.Application.Contracts.UseCases.Progress;
using GymTracker.Application.DTOs.Progress;
using GymTracker.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymTracker.Application.UseCases.Progress
{
    public class ProgressService : IProgressService
    {
        private readonly IWorkoutRepository _workoutRepository;

        public ProgressService(IWorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }


        public async Task<MonthlyProgressResponse> GetMonthlyProgressAsync(int userId, int year, int month, CancellationToken cancellationToken)
        {
            if (month < 1 || month > 12)
                throw new ArgumentException("Month must be between 1 and 12.");

            var daysInMonth = DateTime.DaysInMonth(year, month);
            var rangeStart = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
            var rangeEnd = new DateTime(year, month, daysInMonth, 23, 59, 59, DateTimeKind.Utc);

            var workouts = await _workoutRepository.GetByUserIdAndDateRangeAsync(userId, rangeStart, rangeEnd, cancellationToken);

            var firstDayOfMonth = DateOnly.FromDateTime(rangeStart);
            var lastDayOfMonth = DateOnly.FromDateTime(rangeEnd);
            var weeks = BuildWeeks(firstDayOfMonth, lastDayOfMonth, workouts);

            return new MonthlyProgressResponse(year, month, weeks);
        }

        private static List<WeeklyStats> BuildWeeks(DateOnly firstDayOfMonth, DateOnly lastDayOfMonth, List<Workout> workouts)
        {
            var weeks = new List<WeeklyStats>();

            var cursor = firstDayOfMonth.AddDays(-DayOffsetFromMonday(firstDayOfMonth.DayOfWeek));

            var weekIndex = 1;
            while (cursor <= lastDayOfMonth)
            {
                var theoreticalWeekStart = cursor;
                var theoreticalWeekEnd = cursor.AddDays(6);

                var weekStart = theoreticalWeekStart < firstDayOfMonth ? firstDayOfMonth : theoreticalWeekStart;
                var weekEnd = theoreticalWeekEnd > lastDayOfMonth ? lastDayOfMonth : theoreticalWeekEnd;

                var workoutsInWeek = workouts
                    .Where(w =>
                    {
                        var performedDate = DateOnly.FromDateTime(w.PerformedAt);
                        return performedDate >= weekStart && performedDate <= weekEnd;
                    })
                    .ToList();

                weeks.Add(new WeeklyStats(
                    WeekIndex: weekIndex,
                    StartDate: weekStart,
                    EndDate: weekEnd,
                    WorkoutCount: workoutsInWeek.Count,
                    TotalDurationMinutes: workoutsInWeek.Sum(w => w.DurationMinutes),
                    AverageIntensity: workoutsInWeek.Count > 0 ? workoutsInWeek.Average(w => w.Intensity) : null,
                    AverageFatigue: workoutsInWeek.Count > 0 ? workoutsInWeek.Average(w => w.Fatigue) : null
                ));

                cursor = cursor.AddDays(7);
                weekIndex++;
            }

            return weeks;
        }

        private static int DayOffsetFromMonday(DayOfWeek dayOfWeek)
        {
            return dayOfWeek == DayOfWeek.Sunday ? 6 : (int)dayOfWeek - 1;
        }
    }
}
