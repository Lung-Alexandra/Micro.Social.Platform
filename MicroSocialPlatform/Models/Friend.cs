using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroSocialPlatform.Models;

public class Friend
{
    [Key]
    public int FriendId { get; set; }


    public string User1Id { get; set; }
    [ForeignKey("User1Id")]
    public virtual AppUser User1 { get; set; }

    public string? User2Id { get; set; }
    [ForeignKey("User2Id")]
    public virtual AppUser? User2 { get; set; }

    public DateTime StartDate { get; set; }
    
}