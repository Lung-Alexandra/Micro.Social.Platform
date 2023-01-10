using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroSocialPlatform.Models;

public enum FriendshipStatus
{
    Pending,
    Accepted
}

// This class models the friendship between two users in the app.
public class Friendship
{
    [Key] public int Id { get; set; }

    // The first user, the one that sends the request.
    public string? User1Id { get; set; }

    // The navigation property to the first user. 
    public AppUser? User1 { get; set; }

    // The second user, the one that receives the request and needs to accept it.    
    public string? User2Id { get; set; }

    // The navigation property to the second user. 
    public AppUser? User2 { get; set; }

    // The date when the friendship was accepted. If the friendship is pending,
    // this date is null.
    public DateTime? StartDate { get; set; }

    // The status can be either pending or accepted.
    public FriendshipStatus Status { get; set; }
}