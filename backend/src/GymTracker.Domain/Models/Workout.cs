using System;
using System.Collections.Generic;
using System.Text;

namespace GymTracker.Domain.Models
{
    public class Workout
    {
        public int Id { get; set; }
        public required ExerciseType ExerciseType { get; set; }
        public required int DurationMinutes { get; set; }
        public required int CaloriesBurned { get; set; }
        public required int Intensity { get; set; }
        public required int Fatigue { get; set; }
        public string? Notes { get; set; }
        public required DateTime PerformedAt { get; set; }


        public int UserId { get; set; }
        public User User { get; set; } = null!;


    }
}
