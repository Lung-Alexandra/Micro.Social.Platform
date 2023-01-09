namespace MicroSocialPlatform.Models;

public class FriendsView
{

    // The list of users to show on the page.
    public List<AppUser> Users;

    public FriendsView(List<AppUser> users)
    {
        Users = users;
    }
}