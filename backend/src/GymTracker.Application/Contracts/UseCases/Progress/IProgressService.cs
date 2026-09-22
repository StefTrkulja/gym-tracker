using GymTracker.Application.DTOs.Progress;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymTracker.Application.Contracts.UseCases.Progress
{
    public interface IProgressService
    {
      Task<MonthlyProgressResponse> GetMonthlyProgressAsync(int userId, int year, int month, CancellationToken cancellationToken);

    }
}
