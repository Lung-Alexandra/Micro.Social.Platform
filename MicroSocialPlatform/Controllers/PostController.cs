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

    // Shows a post given by id.
    public IActionResult Index(int id)
    {
        Post post;
        try
        {
            post = _db.Posts.Include("User").First(p => p.Id == id);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("The post does not exist!"));
        }

        return View(post);
    }

    // Only users and admins can create posts.
    [Authorize(Roles = "User,Admin")]
    [HttpPost]
    public IActionResult New(Post post)
    {
        post.UserId = _userManager.GetUserId(User);
        post.Date = DateTime.Now;
        if (ModelState.IsValid)
        {
            _db.Posts.Add(post);
            _db.SaveChanges();
            return RedirectToRoute("home");
        }

        return View();
    }

    [Authorize(Roles = "User,Admin")]
    [HttpGet]
    // Edit the post given by the id.
    public IActionResult Edit(int id)
    {
        Post post;
        // Get the post from the database with the corresponding id.
        try
        {
            post = _db.Posts.Include("User").First(p => p.Id == id);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("The post does not exist!"));
        }

        // Check if user is an admin or owns the post.
        if (_userManager.GetUserId(User) == post.UserId || User.IsInRole("Admin"))
        {
            return View(post);
        }

        return View("MyError", new ErrorView("Cannot edit another user's posts!"));
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPost]
    // Edit the post given by the id.
    public IActionResult Edit(int id, Post newPost)
    {
        Post post;
        // Get the post from the database with the corresponding id.
        try
        {
            post = _db.Posts.Include("User").First(p => p.Id == id);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("The post does not exist!"));
        }

        if (ModelState.IsValid)
        {
            // Check if user is an admin or owns the post.
            if (_userManager.GetUserId(User) == post.UserId || User.IsInRole("Admin"))
            {
                post.Title = newPost.Title;
                post.Content = newPost.Content;
                _db.SaveChanges();
                return RedirectToAction("Index", routeValues: new { id });
            }

            return View("MyError", new ErrorView("Cannot edit another user's posts!"));
        }

        return View(post);
    }
}