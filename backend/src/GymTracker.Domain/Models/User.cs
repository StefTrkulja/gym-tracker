namespace GymTracker.Domain.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHashed { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public ICollection<Workout> Workouts { get; set; } = new List<Workout>();
    public bool IsGoogleAccount { get; set; } = false;

}