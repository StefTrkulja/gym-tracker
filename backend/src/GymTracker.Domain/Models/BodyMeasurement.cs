using System;
using System.Collections.Generic;
using System.Text;

namespace GymTracker.Domain.Models
{
    public class BodyMeasurement
    {
        public int Id { get; set;}
        public required decimal HeightCm { get; set; }
        public required decimal WeightKg { get; set; }
        public required DateTime MeasuredAt { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;


    }
}
