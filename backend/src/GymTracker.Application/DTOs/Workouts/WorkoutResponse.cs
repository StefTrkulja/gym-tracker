using GymTracker.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymTracker.Application.DTOs.Workouts;
    public record WorkoutResponse(
    int Id,
    ExerciseType ExerciseType,
    int DurationMinutes,
    int CaloriesBurned,
    int Intensity,
    int Fatigue,
    string? Notes,
    DateTime PerformedAt
);

