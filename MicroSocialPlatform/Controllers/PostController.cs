using Microsoft.AspNetCore.Authorization;
using MicroSocialPlatform.Models;
using MicroSocialPlatform.Data;
using MicroSocialPlatform.Misc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace MicroSocialPlatform.Controllers;

// This is the controller for the posts.
public class PostController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<AppUser> _userManager;
    private readonly IWebHostEnvironment _web_env;

    public PostController(ApplicationDbContext db, UserManager<AppUser> userManager, IWebHostEnvironment web_env)
    {
        _db = db;
        _userManager = userManager;
        _web_env = web_env;
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
                .ThenInclude(p => p.UserProfile)
                .Include(p => p.Comments.OrderByDescending(c => c.Date))
                .ThenInclude(c => c.User)
                .ThenInclude(u => u.UserProfile)
                .First(p => p.Id == id);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("The post does not exist!"));
        }

        // Count the number of comments.
        post.numComments = _db.Comments.Count(c => c.PostId == id);
        // Only the owner or admins can edit the post.
        post.userCanEdit = _userManager.GetUserId(User) == post.UserId || User.IsInRole("Admin");

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
                .ThenInclude(u => u.UserProfile)
                .First(p => p.Id == new_comment.PostId);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("The post does not exist!"));
        }

        // Count the number of comments.
        post.numComments = _db.Comments.Count(c => c.PostId == post.Id);
        // Only the owner or admins can edit the post.
        post.userCanEdit = _userManager.GetUserId(User) == post.UserId || User.IsInRole("Admin");


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
            // Redirect to the newly created post.
            return RedirectToAction("Index", new { id = post.Id });
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
    public async Task<IActionResult> Edit(int id, Post newPost, IFormFile? postImage)
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
                // Check if there was an image in the form.
                if (postImage!=null && postImage.Length > 0)
                {
                    var storagePath = IOHelper.getImageFilePath(_web_env, postImage.FileName);
                    var databaseString = IOHelper.getImageDatabasePath(postImage.FileName);
                    IOHelper.writeToPath(storagePath,postImage);
                    post.ImageFilename = databaseString;
                }

                post.Title = newPost.Title;
                post.Content = newPost.Content;
                _db.SaveChanges();
                return RedirectToAction("Index", new { id });
            }

            return View("MyError", new ErrorView("Cannot edit another user's posts!"));
        }

        return View(post);
    }


    // Need to be a user or an admin to delete a post.
    [Authorize(Roles = "User,Admin")]
    [HttpPost]
    // Delete the post by id.
    public IActionResult Delete(int id)
    {
        Post toDelete;
        // Find the post in the database.
        try
        {
            // Also include comments to delete them afterwards.
            toDelete = _db.Posts
                .Include(p => p.Comments)
                .First(p => p.Id == id);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("That post does not exist!"));
        }

        // Check if user is an admin or owns the post.
        if (_userManager.GetUserId(User) == toDelete.UserId || User.IsInRole("Admin"))
        {
            // Delete the comments of the post.
            _db.Comments.RemoveRange(toDelete.Comments);
            // Delete the post.
            _db.Posts.Remove(toDelete);
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        return View("MyError", new ErrorView("Cannot delete another user's posts!"));
    }
}