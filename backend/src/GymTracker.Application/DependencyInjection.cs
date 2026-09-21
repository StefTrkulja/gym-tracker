using Microsoft.Extensions.DependencyInjection;
using GymTracker.Application.Contracts.UseCases.Auth;
using GymTracker.Application.Contracts.UseCases.Users;
using GymTracker.Application.UseCases.Auth;
using GymTracker.Application.UseCases.Users;

namespace GymTracker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        
        return services;
    }
}