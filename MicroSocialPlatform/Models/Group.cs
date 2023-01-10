using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroSocialPlatform.Models;

// This class models a group in the application. 
public class Group
{
    [Key] public int Id { get; set; }

    [Required(ErrorMessage = "The group needs a name!")]
    [MinLength(4, ErrorMessage = "The name must have at least 4 characters!")]
    [MaxLength(60, ErrorMessage = "The name must not exceed 60 characters.")]
    // The group name.
    public string Name { get; set; }

    // The group description.
    [StringLength(100, ErrorMessage = "The description exceeds 100 characters")]
    public string? Description { get; set; }

    // When was the group created.
    public DateTime CreationTime { get; set; }

    // The id of the user who created the group.
    public string? UserId { get; set; }

    // The navigation property to the user who created the group.
    public AppUser? User { get; set; }

    // The list of memberships.
    public List<GroupMembership>? Memberships { get; set; }
    
    // The list of messages in this group.
    public List<Message>? Messages{ get; set; }

    [NotMapped]
    // The membership of the current user to this group. It can not exist, so it can be null.
    public GroupMembership? UserMembership;

    [NotMapped]
    public bool UserCanEdit;
}