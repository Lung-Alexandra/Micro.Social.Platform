using System.ComponentModel.DataAnnotations;

namespace MicroSocialPlatform.Models;

public class Message
{
    [Key] public int Id { get; set; }

    //Message itself
    [Required(ErrorMessage = "Message needs to have a content")]
    [MaxLength(60, ErrorMessage = "The content must not exceed 60 characters.")]
    public string Content { get; set; }

    // When was the message sent.
    public DateTime SentTime { get; set; }

    // The id of the user who sent message.
    public string? UserId { get; set; }

    // The navigation property to the user who sent the message.
    public AppUser? User { get; set; }

    // The id of the group where the message was sent.
    public int? GroupId { get; set; }

    // The navigation property to the group where the message was sent.
    public Group? Group { get; set; }
}