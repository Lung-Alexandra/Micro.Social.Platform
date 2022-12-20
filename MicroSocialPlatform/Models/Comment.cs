namespace MicroSocialPlatform.Models;
using System.ComponentModel.DataAnnotations;
// This class models a comment in the application.
public class Comment
{
        [Key] 
        // The id of the comment.
        public int Id { get; set; }
        
        [Required(ErrorMessage="The post needs a content.")]
        [StringLength(300,ErrorMessage = "The content exceeds 300 characters")]
        [MinLength(10,ErrorMessage = "The content must have at least 10 characters")]
        // The content of the comment.
        public string Content { get; set; }
     
        // The date of the comment.
        public DateTime Date { get; set; }
     
        // The id of the owner.
        public string? UserId{ get; set; }

        // The user owning the comment.
        public AppUser? User;
        
        // The id of the post.
        public string? PostId{ get; set; }

        // The post that contains the comment.
        public Post? Post;
}
