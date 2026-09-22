using System;
using System.Collections.Generic;
using System.Text;

namespace GymTracker.Application.DTOs.Progress
{
    public record MonthlyProgressResponse(
    int Year,
    int Month,
    List<WeeklyStats> Weeks
);
}
