using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MicroSocialPlatform.Models;

public enum MembershipStatus
{
    Member,
    Admin,
    Pending,
};

// This class models a group membership. 
public class GroupMembership
{
    [Key] public int Id { get; set; }

    // The user who owns the membership.
    public string UserId { get; set; }

    // The navigation property to the user.
    public AppUser? User { get; set; }

    // The group to which the user has a membership. 
    public int GroupId { get; set; }

    // The navigation property to the group.
    public Group? Group { get; set; }

    // The time that the membership was accepted. If it was not accepted,
    // this value is null.
    public DateTime? JoinDate { get; set; }

    // The status of the membership.
    public MembershipStatus Status { get; set; }

    [NotMapped]
    // If the current user can modify the membership.
    public bool UserCanModify;
}