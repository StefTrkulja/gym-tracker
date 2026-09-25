using Microsoft.Extensions.DependencyInjection;
using GymTracker.Application.Contracts.UseCases.Auth;
using GymTracker.Application.Contracts.UseCases.Users;
using GymTracker.Application.UseCases.Auth;
using GymTracker.Application.UseCases.Users;
using GymTracker.Application.Contracts.UseCases.Workouts;
using GymTracker.Application.UseCases.Workouts;
using GymTracker.Application.Contracts.UseCases.Progress;
using GymTracker.Application.UseCases.Progress;

namespace GymTracker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IWorkoutService, WorkoutService>();
        services.AddScoped<IProgressService, ProgressService>();  
        return services;
    }
}