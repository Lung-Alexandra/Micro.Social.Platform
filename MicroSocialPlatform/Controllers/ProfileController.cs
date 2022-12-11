using MicroSocialPlatform.Data;
using Microsoft.AspNetCore.Authorization;
using MicroSocialPlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MicroSocialPlatform.Controllers;

using Microsoft.AspNetCore.Mvc;

public class ProfileController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<AppUser> _userManager;

    public ProfileController(ApplicationDbContext db, UserManager<AppUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    // Shows the profile of the user.
    [Authorize(Roles = "User,Admin")]
    public IActionResult Index()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            ViewBag.Error = "The user does not exist.";
            return View("MyError");
        }

        // Try to read the user data from the database.
        try
        {
            var user = _db.Users.Include("UserProfile").First(x => x.Id == userId);
            ViewBag.Profile = user.UserProfile;
            ViewBag.User = user;
        }
        catch (InvalidOperationException e)
        {
            ViewBag.Error = "The user does not exist.";
            return View("MyError");
        }
        return View();
    }

    // Edits the profile of the user.
    [Authorize(Roles = "User,Admin")]
    public IActionResult Edit()
    {
        return View();
    }
}