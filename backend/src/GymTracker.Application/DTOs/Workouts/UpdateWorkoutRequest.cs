using GymTracker.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymTracker.Application.DTOs.Workouts;
    public record UpdateWorkoutRequest(
    [Required] ExerciseType ExerciseType,
    [Required][Range(1, 1440)] int DurationMinutes,
    [Required][Range(0, 20000)] int CaloriesBurned,
    [Required][Range(1, 10)] int Intensity,
    [Required][Range(1, 10)] int Fatigue,
    [MaxLength(1000)] string? Notes,
    [Required] DateTime PerformedAt
);

