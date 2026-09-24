using System;
using System.Collections.Generic;
using System.Text;

namespace GymTracker.Application.DTOs.BodyMeasurements
{
    public record BodyMeasurementResponse(
    int Id,
    decimal HeightCm,
    decimal WeightKg,
    DateTime MeasuredAt,
    double Bmi
    );
}
