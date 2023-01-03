using MicroSocialPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroSocialPlatform.Controllers;

using Data;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;

    public HomeController(ApplicationDbContext db)
    {
        _db = db;
    }

    // Show all posts.
    public IActionResult Index()
    {
        // Get the list of posts and comments.
        var posts = _db.Posts
            .Include(p => p.User)
            .ThenInclude(p=>p.UserProfile)
            .Include(p => p.Comments.OrderByDescending(c => c.Date))
            .ThenInclude(c => c.User).ToList();
        var model = new HomeView(posts);
        return View(model);
    }
}