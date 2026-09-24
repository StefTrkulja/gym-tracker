using GymTracker.Infrastructure.Persistence;
using GymTracker.Infrastructure.Persistence.Users;
using GymTracker.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GymTracker.Application.Contracts.Infrastructure;
using GymTracker.Application.Contracts.Persistence;
using GymTracker.Infrastructure.Persistence.Workouts;
using GymTracker.Infrastructure.Persistence.BodyMeasurements;

namespace GymTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();
        });

        //Services
        services.AddSingleton<ITokenService, TokenService>();

        //Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWorkoutRepository, WorkoutRepository>();
        services.AddScoped<IBodyMeasurementRepository, BodyMeasurementRepository>();
        
        return services;
    }
}
