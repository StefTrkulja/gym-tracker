using System;
using System.Collections.Generic;
using System.Text;

namespace GymTracker.Application.DTOs.Progress;
    public record WeeklyStats (
        int WeekIndex,
        DateOnly StartDate,
        DateOnly EndDate,
        int WorkoutCount,
        int TotalCaloriesBurned,
        int TotalDurationMinutes,
        double? AverageIntensity,
        double? AverageFatigue
    );
