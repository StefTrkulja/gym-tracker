using GymTracker.Application.Contracts.Persistence;
using GymTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymTracker.Infrastructure.Persistence.BodyMeasurements
{
    public class BodyMeasurementRepository : IBodyMeasurementRepository
    {
        private readonly DataContext _context;

        public BodyMeasurementRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<BodyMeasurement> CreateAsync(BodyMeasurement measurement, CancellationToken cancellationToken)
        {
            _context.BodyMeasurements.Add(measurement);
            await _context.SaveChangesAsync(cancellationToken);
            return measurement;
        }

        public async Task<BodyMeasurement> UpdateAsync(BodyMeasurement measurement, CancellationToken cancellationToken)
        {
            _context.BodyMeasurements.Update(measurement);
            await _context.SaveChangesAsync(cancellationToken);
            return measurement;
        }

        public async Task DeleteAsync(BodyMeasurement measurement, CancellationToken cancellationToken)
        {
            _context.BodyMeasurements.Remove(measurement);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<BodyMeasurement?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.BodyMeasurements.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public async Task<List<BodyMeasurement>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            return await _context.BodyMeasurements
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.MeasuredAt)
                .ToListAsync(cancellationToken);
        }
    }
}
