using GymTracker.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymTracker.Application.Contracts.Persistence
{
    public interface IBodyMeasurementRepository
    {
        Task<BodyMeasurement> CreateAsync(BodyMeasurement measurement, CancellationToken cancellationToken);
        Task<BodyMeasurement> UpdateAsync(BodyMeasurement measurement, CancellationToken cancellationToken);
        Task DeleteAsync(BodyMeasurement measurement, CancellationToken cancellationToken);
        Task<BodyMeasurement?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<BodyMeasurement>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken);
    }
}
