namespace MicroSocialPlatform.Models;


// The model we send to the home index.
public class HomeView 
{
    // It contains the list of posts.
    public List<Post> Posts { get; set; }

    public HomeView(List<Post> posts)
    {
        Posts = posts;
    }
}