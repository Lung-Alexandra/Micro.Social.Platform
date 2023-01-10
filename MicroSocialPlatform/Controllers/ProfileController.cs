using MicroSocialPlatform.Data;
using Microsoft.AspNetCore.Authorization;
using MicroSocialPlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MicroSocialPlatform.Misc;

namespace MicroSocialPlatform.Controllers;

using Microsoft.AspNetCore.Mvc;

public class ProfileController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<AppUser> _userManager;
    private readonly List<Gender> _genderList;
    private readonly IWebHostEnvironment _webEnv;
    private readonly SignInManager<AppUser> _signInManager;

    public ProfileController(ApplicationDbContext db, UserManager<AppUser> userManager, IWebHostEnvironment webEnv,
        SignInManager<AppUser> sinSignInManager)
    {
        _db = db;
        _userManager = userManager;
        _genderList = new List<Gender> { Gender.Male, Gender.Female, Gender.Unspecified };
        _webEnv = webEnv;
        _signInManager = sinSignInManager;
    }

    // Shows the profile given by id.
    public IActionResult Index(int id)
    {
        Profile profile;
        try
        {
            profile = _db.Profiles.Include(p => p.User).ThenInclude(p => p.UserPosts).First(x => x.Id == id);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("The profile does not exist."));
        }

        string myId = _userManager.GetUserId(User);
        profile.userOwnsProfile = myId == profile.UserId;
        profile.userCanEdit = profile.userOwnsProfile || User.IsInRole("Admin");
        profile.userSent = _db.Friendships.FirstOrDefault(f => f.User1Id == myId && f.User2Id == profile.UserId);
        profile.userReceived = _db.Friendships.FirstOrDefault(f => f.User1Id == profile.UserId && f.User2Id == myId);
        profile.numPosts = profile.User.UserPosts.Count;
        profile.numFriends = _db.Friendships.Count(f =>
            f.Status == FriendshipStatus.Accepted && (f.User1Id == profile.UserId || f.User2Id == profile.UserId));

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
    public IActionResult Edit(int id, Profile newProfile, IFormFile? profileImage)
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
                // Check if there was an image in the form. 
                if (profileImage != null && profileImage.Length > 0)
                {
                    IOHelper.saveImage(_webEnv, profileImage);
                    profile.ImageFilename = IOHelper.getImageDatabasePath(profileImage.FileName);
                }


                profile.AboutMe = newProfile.AboutMe;
                profile.Gender = newProfile.Gender;
                profile.Visibility = newProfile.Visibility;
                _db.SaveChanges();
                return RedirectToAction("Index", new { id });
            }

            return View("MyError", new ErrorView("You cannot edit another user's profile"));
        }

        ViewBag.GenderList = _genderList;
        return View(profile);
    }

    // Need to be a user or an admin to delete a profile.
    [Authorize(Roles = "User,Admin")]
    [HttpPost]
    // Delete the profile by id.
    public async Task<ActionResult> Delete(int id)
    {
        Profile profile;
        // Try to read the profile from the database.
        try
        {
            profile = _db.Profiles
                .Include(u => u.User).ThenInclude(u => u.UserReceivedFriendships)
                .Include(u => u.User).ThenInclude(u => u.UserSentFriendships)
                .Include(u => u.User).ThenInclude(u=>u.UserGroups).ThenInclude(g => g.Memberships)
                .Include(u => u.User).ThenInclude(u=>u.UserGroups).ThenInclude(g => g.Messages)
                .Include(u=>u.User).ThenInclude(u=>u.UserPosts).ThenInclude(p=> p.Comments)
                .First(x => x.Id == id);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("The profile does not exist."));
        }

        // Check if this is our profile.
        bool myUser = _userManager.GetUserId(User) == profile.UserId;
        // Check if user is an admin or owns the profile.
        if (myUser || User.IsInRole("Admin"))
        {
            _db.Friendships.RemoveRange(profile.User.UserReceivedFriendships);
            _db.Friendships.RemoveRange(profile.User.UserSentFriendships);
            // Delete the user.
            await _userManager.DeleteAsync(profile.User);
            _db.SaveChanges();
            if (myUser)
            {
                // If we deleted our own user, sign out.
                await _signInManager.SignOutAsync();
            }

            return RedirectToAction("Index", "Start");
        }

        return View("MyError", new ErrorView("Cannot delete another user's profile!"));
    }
}