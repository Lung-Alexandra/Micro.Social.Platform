using Microsoft.AspNetCore.Identity;

namespace MicroSocialPlatform.Models;

// This class defines the application user.
public class AppUser : IdentityUser
{
    // The date the user registered.
    public DateTime RegisteredAt { get; set; }

    // The profile of the user.
    public Profile UserProfile { get; set; }
}