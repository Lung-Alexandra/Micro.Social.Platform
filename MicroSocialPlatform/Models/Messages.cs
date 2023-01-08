using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace MicroSocialPlatform.Models;

public class Messages
{
    [Key]
    public int Id { get; set; }
    
    //Message itself
    [Required(ErrorMessage = "Message needs to have a content")]
    [MinLength(1,ErrorMessage = "The content must have at least 1 characters!")]
    [MaxLength(60,ErrorMessage = "The content must not exceed 60 characters.")]
    public string Message { get; set; }
    
    // When was the message sent.
    public DateTime SentTime { get; set;}
    
    // The id of the user who sent message.
    public string? UserId;

    // The navigation property to the user who sent message.
    public AppUser? User;
    
    // The id of the group where message was sent.
    public int? GroupId;

    // The navigation property to the group where message was sent.
    public Group? Group;
}