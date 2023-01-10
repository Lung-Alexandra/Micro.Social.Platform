using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroSocialPlatform.Models;

public enum Gender
{
    Male,
    Female,
    Unspecified
}

// The visibility of the profile.
public enum Visibility
{
    Public,
    Private
}

// This class defines the profile of the user. 
public class Profile
{
    [Key]
    // The id of the profile.
    public int Id { get; set; }

    // The id of the user owning the profile.
    public string? UserId { get; set; }

    // The user owning the profile.
    public AppUser? User { get; set; }

    [StringLength(100, ErrorMessage = "About me exceeds 100 characters.")]
    // Data about the user.
    public string? AboutMe { get; set; }

    // The gender of the user.
    public Gender Gender { get; set; }

    // If the profile is private or not.
    public Visibility Visibility { get; set; }

    // The file name of the profile image.
    public string? ImageFilename { get; set; }

    [NotMapped]
    // If the current user owns this profile.
    public bool userOwnsProfile;

    [NotMapped]
    // If the current can edit this profile.
    public bool userCanEdit;

    [NotMapped]
    // The friendship received by the current user, send by the user with this profile. 
    public Friendship? userReceived;

    [NotMapped]
    // The friendship sent by the current user, to the user with this profile. 
    public Friendship? userSent;

    [NotMapped]
    // The number of posts made by the user owning this profile.
    public int numPosts;

    [NotMapped]
    // The number of friends of the user owning this profile.
    public int numFriends;
}