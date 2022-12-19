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
    private readonly List<Gender> _genderList;

    public ProfileController(ApplicationDbContext db, UserManager<AppUser> userManager)
    {
        _db = db;
        _userManager = userManager;
        _genderList = new List<Gender> { Gender.Male, Gender.Female, Gender.Unspecified };
    }

    // Shows the profile given by id.
    public IActionResult Index(int id)
    {
        Profile profile;
        try
        {
            profile = _db.Profiles.Include("User").First(x => x.Id == id);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("The profile does not exist."));
        }

        return View(profile);
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
            return View("MyError", new ErrorView("The profile does not exist."));
        }

        // Only the user owning the profile or an admin can change the profile.
        var userId = _userManager.GetUserId(User);
        if (userId == profile.User.Id || User.IsInRole("Admin"))
        {
            ViewBag.GenderList = _genderList;
            return View(profile);
        }

        return View("MyError", new ErrorView("You cannot edit another user's profile"));
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPost]
    // Edits the profile with the given id.
    public IActionResult Edit(int id, Profile newProfile)
    {
        Profile profile;
        // Try to read the profile from the database.
        try
        {
            profile = _db.Profiles.Include("User").First(x => x.Id == id);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("The profile does not exist."));
        }

        // Check if the model respects all constraints.
        if (ModelState.IsValid)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == profile.UserId || User.IsInRole("Admin"))
            {
                profile.AboutMe = newProfile.AboutMe;
                profile.Gender = newProfile.Gender;
                _db.SaveChanges();
                return RedirectToAction("Index", new { id });
            }

            return View("MyError", new ErrorView("You cannot edit another user's profile"));
        }

        ViewBag.GenderList = _genderList;
        return View(profile);
    }
}