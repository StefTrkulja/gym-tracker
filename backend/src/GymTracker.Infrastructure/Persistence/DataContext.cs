using Microsoft.EntityFrameworkCore;
using GymTracker.Domain.Models;

namespace GymTracker.Infrastructure.Persistence;

public sealed class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Workout> Workouts { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}