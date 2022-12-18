using Microsoft.AspNetCore.Authorization;
using MicroSocialPlatform.Models;
using MicroSocialPlatform.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace MicroSocialPlatform.Controllers;

// This is the controller for the posts.
public class PostController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<AppUser> _userManager;

    public PostController(ApplicationDbContext db, UserManager<AppUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    // Only users and admins can create posts.
    [Authorize(Roles = "User,Admin")]
    [HttpGet]
    public IActionResult New()
    {
        return View();
    }

    // Only users and admins can create posts.
    [Authorize(Roles = "User,Admin")]
    [HttpPost]
    public IActionResult New(Post post)
    {
        post.UserId= _userManager.GetUserId(User);
        post.Date = DateTime.Now;
        if (ModelState.IsValid)
        {
            _db.Posts.Add(post);
            _db.SaveChanges();
            return RedirectToRoute("home");
        }
        return View();
    }
}