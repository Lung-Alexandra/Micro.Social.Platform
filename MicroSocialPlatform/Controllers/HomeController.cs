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
        var posts = _db.Posts.Include("User").ToList();
        HomeView model = new HomeView(posts);
        return View(model);
    }
}