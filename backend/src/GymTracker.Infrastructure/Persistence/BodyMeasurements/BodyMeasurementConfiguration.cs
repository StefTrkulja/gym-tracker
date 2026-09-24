using GymTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymTracker.Infrastructure.Persistence.BodyMeasurements
{
    public class BodyMeasurementConfiguration : IEntityTypeConfiguration<BodyMeasurement>
    {
        public void Configure(EntityTypeBuilder<BodyMeasurement> builder)
        {
            builder.ToTable("body_measurements");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.HeightCm)
                .HasColumnType("decimal(5,2)");

            builder.Property(m => m.WeightKg)
                .HasColumnType("decimal(5,2)");

            builder.HasIndex(m => new { m.UserId, m.MeasuredAt });

            builder.HasOne(m => m.User)
                .WithMany(u => u.BodyMeasurements)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_BodyMeasurements_Height", "height_cm BETWEEN 50 AND 250");
                t.HasCheckConstraint("CK_BodyMeasurements_Weight", "weight_kg BETWEEN 20 AND 400");
            });
        }
    }
}
