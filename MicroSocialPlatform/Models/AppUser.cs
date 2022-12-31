using Microsoft.AspNetCore.Identity;

namespace MicroSocialPlatform.Models;

// This class defines the application user.
public class AppUser : IdentityUser
{
    // The date the user registered.
    public DateTime RegisteredAt { get; set; }

    // The profile of the user.
    public Profile? UserProfile { get; set; }

    // The posts made by the user.
    public List<Post>? UserPosts { get; set; }
    // The commnets made by the user.
    public List<Comment>? UserComments { get; set; }
    // The friends of the user.
    public List<Friend>? UserFriends { get; set; }
    // The groups of the user.
    public List<Group>? UserGroups { get; set; }
}