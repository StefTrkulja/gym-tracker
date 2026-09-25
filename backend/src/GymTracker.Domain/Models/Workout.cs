using GymTracker.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymTracker.Domain.Models
{
    public class Workout
    {
        public int Id { get; private set; }
        public ExerciseType ExerciseType { get; private set; }
        public int DurationMinutes { get; private set; }
        public int CaloriesBurned { get; private set; }
        public int Intensity { get; private set; }
        public int Fatigue { get; private set; }
        public string? Notes { get; private set; }
        public DateTime PerformedAt { get; private set; }


        public int UserId { get; private set; }
        public User User { get; private set; } = null!;

        private Workout() { }

        public Workout(int userId, ExerciseType exceriseType, int durationMinutes, int caloriesBurned, int intensity, int fatigue, string? notes, DateTime performedAt)
        {
            UserId = userId;
            Apply(exceriseType, durationMinutes, caloriesBurned, intensity, fatigue, notes, performedAt);
        }


        public bool IsOwnedBy(int userId) => UserId == userId;

        public void Update(ExerciseType exerciseType, int durationMinutes, int caloriesBurned,int intensity, int fatigue, string? notes, DateTime performedAt)
            => Apply(exerciseType, durationMinutes, caloriesBurned, intensity, fatigue, notes, performedAt);
       
        private void Apply(ExerciseType exerciseType, int durationMinutes, int caloriesBurned, int intensity, int fatique, string? notes, DateTime performedAt)
        {
            if (durationMinutes <= 0)
            {
                throw new DomainException("Duration must be greater than 0.");
            }
            if (caloriesBurned < 0)
            {
                throw new DomainException("Calories burned can't be negative");
            }
            if (intensity < 1 || intensity > 10)
            {
                throw new DomainException("Intensity must be 1 - 10");
            }
            if (fatique < 1 || fatique > 10)
            {
                throw new DomainException("Fatique must be 1 - 10");
            }
            ExerciseType = exerciseType;
            DurationMinutes = durationMinutes;
            CaloriesBurned = caloriesBurned;
            Intensity = intensity;
            Fatigue = fatique;
            Notes = notes;
            PerformedAt = performedAt;
        }

    }
}
