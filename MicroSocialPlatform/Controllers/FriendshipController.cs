using Microsoft.AspNetCore.Authorization;
using MicroSocialPlatform.Models;
using MicroSocialPlatform.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace MicroSocialPlatform.Controllers;

public class FriendshipController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<AppUser> _userManager;

    public FriendshipController(ApplicationDbContext db, UserManager<AppUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }


    [Authorize(Roles = "User,Admin")]
    [HttpPost]
    // Creates a new friendship from the current user to the given user.
    public IActionResult New(string id)
    {
        string from = _userManager.GetUserId(User);
        AppUser to;
        try
        {
            to = _db.Users
                .Include(u => u.UserProfile)
                .First(u => u.Id == id);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("That user does not exist!"));
        }

        Friendship friendship = new Friendship();
        friendship.User1Id = from;
        friendship.User2Id = id;
        friendship.Status = FriendshipStatus.Pending;
        friendship.StartDate = null;
        _db.Friendships.Add(friendship);
        _db.SaveChanges();
        return RedirectToAction("Index", "Profile", new { id = to.UserProfile.Id });
    }

    // Shows all friends for the given profile.
    [HttpGet]
    public IActionResult Index(int id)
    {
        Profile profile;
        try
        {
            profile = _db.Profiles
                .Include(p => p.User).ThenInclude(u => u.UserSentFriendships)
                .Include(p => p.User).ThenInclude(u => u.UserReceivedFriendships)
                .First(p => p.Id == id);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("That profile does not exist!"));
        }

        return View();
    }
}