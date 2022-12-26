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
    [HttpGet]
    public IActionResult Index(int id)
    {
        Post post;
        try
        {
            post = _db.Posts
                .Include(p => p.User)
                .Include(p => p.Comments.OrderByDescending(c => c.Date))
                .ThenInclude(c => c.User).First(p => p.Id == id);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("The post does not exist!"));
        }

        return View(post);
    }

    [HttpPost]
    [Authorize(Roles = "User,Admin")]
    public IActionResult Index([FromForm] Comment new_comment)
    {
        Post post;
        try
        {
            post = _db.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .ThenInclude(c => c.User)
                .First(p => p.Id == new_comment.PostId);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("The post does not exist!"));
        }

        new_comment.Date = DateTime.Now;
        new_comment.UserId = _userManager.GetUserId(User);

        if (ModelState.IsValid)
        {
            _db.Comments.Add(new_comment);
            _db.SaveChanges();
            return RedirectToAction("Index", new { id = post.Id });
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
                return RedirectToAction("Index", new { id });
            }

            return View("MyError", new ErrorView("Cannot edit another user's posts!"));
        }

        return View(post);
    }
}