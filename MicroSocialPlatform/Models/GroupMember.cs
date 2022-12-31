using System.ComponentModel.DataAnnotations;


namespace MicroSocialPlatform.Models;

public class GroupMember
{
    [Key]
    public int GroupMemberId { get; set; }
   
    public string UserId { get; set; }
    public virtual AppUser User { get; set; }
   
    public int GroupId { get; set; }
    public virtual Group Group { get; set; }

    public DateTime? JoinDate { get; set; }
    public MemberStatusFlag Status { get; set; }

    public enum MemberStatusFlag
{
    Member,
    Admin,
    Pending,
    Invited
};
}