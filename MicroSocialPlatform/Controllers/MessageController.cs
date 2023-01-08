using MicroSocialPlatform.Data;
using Microsoft.AspNetCore.Identity;
using MicroSocialPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroSocialPlatform.Controllers;

public class MessageController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<AppUser> _userManager;

    public MessageController(ApplicationDbContext db, UserManager<AppUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }


    [Authorize(Roles = "User,Admin")]
    [HttpGet]
    // Shows the edit page for the message given by id.
    public IActionResult Edit(int id)
    {
        Message message;
        try
        {
            message = _db.Messages.First(m => m.Id == id);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("The message does not exist!"));
        }

        string myId = _userManager.GetUserId(User);

        // Only the owner and admins can edit messages.
        if (myId == message.UserId || User.IsInRole("Admin"))
        {
            return View(message);
        }

        return View("MyError", new ErrorView("You cannot edit another user's messages!"));
    }


    [Authorize(Roles = "User,Admin")]
    [HttpPost]
    // Edits the message using the data received in the post request. 
    public IActionResult Edit(Message edited)
    {
        // Search the new comment by id.
        Message toEdit;
        try
        {
            toEdit = _db.Messages.First(m => m.Id == edited.Id);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("The message does not exist!"));
        }

        // If the model state is valid.
        if (ModelState.IsValid)
        {
            string myId = _userManager.GetUserId(User);
            // Only the owner and admins can edit messages.
            if (myId == toEdit.UserId || User.IsInRole("Admin"))
            {
                toEdit.Content = edited.Content;
                _db.SaveChanges();
                return RedirectToAction("Index", "Group", new { id = toEdit.GroupId });
            }

            return View("MyError", new ErrorView("You cannot edit another user's messages!"));
        }

        // else show validation errors.
        return View(toEdit);
    }
}