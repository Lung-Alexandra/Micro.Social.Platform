namespace MicroSocialPlatform.Models;

// The view model for the people page.
// It holds the users we need to show on the page.
public class PeopleView
{
    // The list of users to show on the page.
    public List<AppUser> Users;

    public PeopleView(List<AppUser> users)
    {
        Users = users;
    }
}