using System.ComponentModel.DataAnnotations.Schema;

namespace MicroSocialPlatform.Models;

using System.ComponentModel.DataAnnotations;

// This class models a post in the application.
public class Post
{
    [Key]
    // The id of the post.
    public int Id { get; set; }

    [Required(ErrorMessage = "The post needs a title.")]
    [StringLength(100, ErrorMessage = "The title exceeds 100 characters")]
    [MinLength(10, ErrorMessage = "The title must have at least 10 characters.")]
    // The title of the post.
    public string Title { get; set; }


    [Required(ErrorMessage = "The post needs a content.")]
    [StringLength(300, ErrorMessage = "The content exceeds 300 characters")]
    [MinLength(30, ErrorMessage = "The content must have at least 30 characters")]
    // The content of the post.
    public string Content { get; set; }

    // The date of the post.
    public DateTime Date { get; set; }

    // The id of the owner.
    public string? UserId { get; set; }

    // The user owning the post.
    public AppUser? User;

    // The comments for a post.
    public List<Comment>? Comments { get; set; }

    [NotMapped]
    // The number of comments in this post.
    public int numComments;

    [NotMapped]
    // If the current user can edit this post.
    public bool userCanEdit;
}