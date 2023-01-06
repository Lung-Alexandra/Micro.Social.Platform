namespace MicroSocialPlatform.Models;

// This model contains the data that needs to be send to the groups page.
public class GroupsView
{
    // The list of groups.
    public List<Group> Groups { get; set; }

    public GroupsView(List<Group> groups)
    {
        Groups = groups;
    }
}