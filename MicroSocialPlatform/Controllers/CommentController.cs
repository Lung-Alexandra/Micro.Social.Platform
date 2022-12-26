using Microsoft.AspNetCore.Authorization;
using MicroSocialPlatform.Models;
using MicroSocialPlatform.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;


namespace MicroSocialPlatform.Controllers;

public class CommentController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<AppUser> _userManager;

    public CommentController(ApplicationDbContext db, UserManager<AppUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    // Only users and admins can create comments.
    [Authorize(Roles = "User,Admin")]
    [HttpPost]
    public IActionResult New(Comment new_comment)
    {
        Post post;
        // Get the post referenced by the comment.
        try
        {
            post = _db.Posts.First(p => p.Id == new_comment.PostId);
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
            return RedirectToAction("Index", "Post", new {id = new_comment.PostId});
        }

        // The model state is not valid.
        return RedirectToAction("Index", "Home");
    }
}