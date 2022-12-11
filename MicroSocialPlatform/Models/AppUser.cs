using Microsoft.AspNetCore.Identity;

namespace MicroSocialPlatform.Models;

// This class defines the application user.
public class AppUser : IdentityUser
{
    // The date the user registered.
    public DateTime RegisteredAt { get; set; }

    // The profile of the user.
    // The user may not have a profile. If it has just been registered,
    // the user does not have a profile. It must be created.
    public Profile? UserProfile { get; set; }
}