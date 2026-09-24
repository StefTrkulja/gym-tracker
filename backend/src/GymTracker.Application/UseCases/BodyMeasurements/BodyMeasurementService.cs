using GymTracker.Application.Contracts.Persistence;
using GymTracker.Application.Contracts.UseCases.BodyMeasurements;
using GymTracker.Application.DTOs.BodyMeasurements;
using GymTracker.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymTracker.Application.UseCases.BodyMeasurements
{
    public class BodyMeasurementService : IBodyMeasurementService
    {
        private readonly IBodyMeasurementRepository _bodyMeasurementRepository;

        public BodyMeasurementService(IBodyMeasurementRepository bodyMeasurementRepository)
        {
            _bodyMeasurementRepository = bodyMeasurementRepository;
        }

        public async Task<BodyMeasurementResponse> CreateAsync(int userId, CreateBodyMeasurementRequest request, CancellationToken cancellationToken)
        {
            var measurement = new BodyMeasurement
            {
                UserId = userId,
                HeightCm = request.HeightCm,
                WeightKg = request.WeightKg,
                MeasuredAt = request.MeasuredAt
            };

            var created = await _bodyMeasurementRepository.CreateAsync(measurement, cancellationToken);
            return ToResponse(created);
        }

        public async Task<BodyMeasurementResponse> UpdateAsync(int userId, int id, UpdateBodyMeasurementRequest request, CancellationToken cancellationToken)
        {
            var measurement = await _bodyMeasurementRepository.GetByIdAsync(id, cancellationToken);
            if (measurement is null)
                throw new KeyNotFoundException($"Body measurement with id {id} not found.");

            if (measurement.UserId != userId)
                throw new UnauthorizedAccessException("You are not allowed to modify this body measurement.");

            measurement.HeightCm = request.HeightCm;
            measurement.WeightKg = request.WeightKg;
            measurement.MeasuredAt = request.MeasuredAt;

            var updated = await _bodyMeasurementRepository.UpdateAsync(measurement, cancellationToken);
            return ToResponse(updated);
        }

        public async Task DeleteAsync(int userId, int id, CancellationToken cancellationToken)
        {
            var measurement = await _bodyMeasurementRepository.GetByIdAsync(id, cancellationToken);
            if (measurement is null)
                throw new KeyNotFoundException($"Body measurement with id {id} not found.");

            if (measurement.UserId != userId)
                throw new UnauthorizedAccessException("You are not allowed to delete this body measurement.");

            await _bodyMeasurementRepository.DeleteAsync(measurement, cancellationToken);
        }

        public async Task<List<BodyMeasurementResponse>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            var measurements = await _bodyMeasurementRepository.GetAllByUserIdAsync(userId, cancellationToken);
            return measurements.Select(ToResponse).ToList();
        }

        private static BodyMeasurementResponse ToResponse(BodyMeasurement measurement)
        {
            var heightMeters = measurement.HeightCm / 100m;
            var bmi = (double)(measurement.WeightKg / (heightMeters * heightMeters));

            return new BodyMeasurementResponse(
                measurement.Id,
                measurement.HeightCm,
                measurement.WeightKg,
                measurement.MeasuredAt,
                Math.Round(bmi, 1)
            );
        }
    }
}
