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

    [Authorize(Roles = "User,Admin")]
    // Shows the profile given by id.
    public IActionResult Index(int id)
    {
        var profile = _db.Profiles.Find(id);
        if (profile == null)
        {
            ViewBag.Error = "The given profile does not exist.";
            return View("MyError");
        }

        ViewBag.Profile = profile;
        return View();
    }

    [Authorize(Roles = "User,Admin")]
    [HttpGet]
    // Shows the edit page for the profile with the given id.
    public IActionResult Edit(int id)
    {
        Profile profile;
        // Try to read the profile from the database.
        try
        {
            profile = _db.Profiles.Include("User").First(x => x.Id == id);
        }
        catch (InvalidOperationException)
        {
            ViewBag.Error = "The profile does not exist.";
            return View("MyError");
        }

        // Only the user owning the profile or an admin can change the profile.
        var userId = _userManager.GetUserId(User);
        if (userId == profile.User.Id || User.IsInRole("Admin"))
        {
            var genderList = new List<Gender> { Gender.Male, Gender.Female, Gender.Unspecified };
            ViewBag.GenderList = genderList;
            return View(profile);
        }

        ViewBag.Error = "You cannot edit another user's profile.";
        return View("MyError");
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPost]
    // Edits the profile with the given id.
    public IActionResult Edit(int id, Profile new_profile)
    {
        Profile profile;
        // Try to read the profile from the database.
        try
        {
            profile = _db.Profiles.Include("User").First(x => x.Id == id);
        }
        catch (InvalidOperationException)
        {
            ViewBag.Error = "The profile does not exist.";
            return View("MyError");
        }

        var userId = _userManager.GetUserId(User);
        if (userId == profile.User.Id || User.IsInRole("Admin"))
        {
            profile.AboutMe = new_profile.AboutMe;
            profile.Gender = new_profile.Gender;
            _db.SaveChanges();
            return RedirectToAction("Index", new { id });
        }

        ViewBag.Error = "You cannot edit another user's profile.";
        return View("MyError");
    }
}