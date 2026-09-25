using GymTracker.Domain.Exceptions;

namespace GymTracker.Domain.Models;

public class User
{
    public int Id { get; private set; }
    public string Username { get; private set; } = null!;
    public  string PasswordHashed { get; private set; } = null!;
    public  string Email { get; private set; } = null!;
    public  string FirstName { get; private set; } = null!;
    public  string LastName { get; private set; } = null!;
    public ICollection<Workout> Workouts { get; private set; } = new List<Workout>();
    public bool IsGoogleAccount { get; private set; }

    private User () { }

    public User(string username, string password, string email, string firstName, string lastName, bool isGoogleAccount = false)
    {
        Username = username;
        PasswordHashed = password;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        IsGoogleAccount = isGoogleAccount;
    }

    public void UpdateProfile(string username, string email, string firstName, string lastName)
    {
        if (IsGoogleAccount && Email != email)
        {
            throw new DomainException("You can't change Google account");
        }

        Username = username;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }
}