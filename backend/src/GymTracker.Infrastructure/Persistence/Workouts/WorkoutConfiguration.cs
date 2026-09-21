using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GymTracker.Domain.Models;

namespace GymTracker.Infrastructure.Persistence.Workouts;

public class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
{
    public void Configure(EntityTypeBuilder<Workout> builder)
    {
        builder.ToTable("workouts");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.ExerciseType)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(w => w.Notes)
            .HasMaxLength(1000);

        builder.HasIndex(w => new { w.UserId, w.PerformedAt });

        builder.HasOne(w => w.User)
            .WithMany(u => u.Workouts)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Workouts_Intensity", "intensity BETWEEN 1 AND 10");
            t.HasCheckConstraint("CK_Workouts_Fatigue", "fatigue BETWEEN 1 AND 10");
            t.HasCheckConstraint("CK_Workouts_Duration", "duration_minutes > 0");
            t.HasCheckConstraint("CK_Workouts_Calories", "calories_burned >= 0");
        });
    }
}