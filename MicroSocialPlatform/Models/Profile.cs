using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroSocialPlatform.Models;

public enum Gender
{
    Male,
    Female,
    Unspecified
}

// This class defines the profile of the user. 
public class Profile
{
    [Key] 
    // The id of the profile.
    public int Id { get; set; }

    // The id of the user owning the profile.
    public string UserId { get; set; }

    // The user owning the profile.
    public AppUser User { get; set; }

    // Data about the user.
    public string? AboutMe { get; set; }

    // The gender of the user.
    public Gender Gender { get; set; }
}