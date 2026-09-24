using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymTracker.Application.DTOs.BodyMeasurements
{
    public record CreateBodyMeasurementRequest(
    [Required][Range(50, 250)] decimal HeightCm,
    [Required][Range(20, 400)] decimal WeightKg,
    [Required] DateTime MeasuredAt
    );
}
