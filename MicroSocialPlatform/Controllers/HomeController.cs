using MicroSocialPlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MicroSocialPlatform.Controllers;

using Data;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<AppUser> _userManager;

    public HomeController(ApplicationDbContext db, UserManager<AppUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    // Show all posts.
    public IActionResult Index()
    {
        // Get the list of posts and comments.
        var posts = _db.Posts
            .Include(p => p.User)
            .ThenInclude(u => u.UserProfile)
            .Include(p => p.Comments.OrderByDescending(c => c.Date))
            .ThenInclude(c => c.User)
            .ThenInclude(u => u.UserProfile)
            .ToList();

        bool admin = User.IsInRole("Admin");
        string myId = _userManager.GetUserId(User);

        foreach (var post in posts)
        {
            // Set the number of comments.
            post.numComments = post.Comments.Count;
            // The post can be edited if the current user is an admin or owns the post.
            post.userCanEdit = myId == post.UserId || admin;
        }

        var model = new HomeView(posts);
        return View(model);
    }
}