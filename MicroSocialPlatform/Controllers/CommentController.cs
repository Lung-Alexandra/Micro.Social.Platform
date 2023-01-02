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

    // Only users and admins can edit comments.
    [Authorize(Roles = "User,Admin")]
    [HttpGet]
    public IActionResult Edit(int id)
    {
        // First find the comment by id.
        Comment toEdit;
        try
        {
            toEdit = _db.Comments.First(c => c.Id == id);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("That comment does not exist!"));
        }

        // Next check if the user is the owner or an admin.
        if (_userManager.GetUserId(User) == toEdit.UserId || User.IsInRole("Admin"))
        {
            return View(toEdit);
        }

        return View("MyError", new ErrorView("You cannot edit another user's comments!"));
    }

    // Only users and admins can edit comments.
    [Authorize(Roles = "User,Admin")]
    [HttpPost]
    public IActionResult Edit(Comment edited)
    {
        // First find the comment by id.
        Comment toEdit;
        try
        {
            toEdit = _db.Comments.First(c => c.Id == edited.Id);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("That comment does not exist!"));
        }

        if (ModelState.IsValid)
        {
            // Check if the user is the owner or an admin.
            if (_userManager.GetUserId(User) == toEdit.UserId || User.IsInRole("Admin"))
            {
                // Edit the comment and redirect to the post owning the comment.
                toEdit.Content = edited.Content;
                _db.SaveChanges();
                return RedirectToAction("Index", "Post", new { id = toEdit.PostId });
            }

            return View("MyError", new ErrorView("You cannot edit another user's comments!"));
        }

        return View(toEdit);
    }
}