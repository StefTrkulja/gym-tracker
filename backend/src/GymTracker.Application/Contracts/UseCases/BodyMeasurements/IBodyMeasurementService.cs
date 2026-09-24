using GymTracker.Application.DTOs.BodyMeasurements;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymTracker.Application.Contracts.UseCases.BodyMeasurements
{
    public interface IBodyMeasurementService
    {
        Task<BodyMeasurementResponse> CreateAsync(int userId, CreateBodyMeasurementRequest request, CancellationToken cancellationToken);
        Task<BodyMeasurementResponse> UpdateAsync(int userId, int id, UpdateBodyMeasurementRequest request, CancellationToken cancellationToken);
        Task DeleteAsync(int userId, int id, CancellationToken cancellationToken);
        Task<List<BodyMeasurementResponse>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken);
    }
}
