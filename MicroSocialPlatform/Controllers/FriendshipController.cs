using MicroSocialPlatform.Data;
using MicroSocialPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        // An user cannot send a friend request to itself.
        if (from == id)
        {
            return View("MyError", new ErrorView("You cannot send a friend request to yourself!"));
        }

        // Get the profile of the target user.
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

        // Check if there already is a connection between this user to the target.
        if (_db.Friendships.Any(f => f.User1Id == from && f.User2Id == id))
        {
            return View("MyError", new ErrorView("You already sent a friend request to that user!"));
        }

        // Check if there already is a connection between the target to this user.
        if (_db.Friendships.Any(f => f.User1Id == id && f.User2Id == from))
        {
            return View("MyError", new ErrorView("That user already sent you a friend request!"));
        }

        Friendship friendship = new Friendship();
        friendship.User1Id = from;
        friendship.User2Id = id;
        friendship.Status = FriendshipStatus.Pending;
        friendship.StartDate = null;
        _db.Friendships.Add(friendship);
        _db.SaveChanges();

        // Redirect to the target user's profile.
        return RedirectToAction("Index", "Profile", new { id = to.UserProfile.Id });
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPost]
    public IActionResult AcceptFriendship(int id)
    {
        string my_id = _userManager.GetUserId(User);

        // Get the friendship by id.
        Friendship friendship;
        try
        {
            friendship = _db.Friendships.First(u => u.FriendshipId == id);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("The friend request does not exist!"));
        }

        if (friendship.User2Id != my_id)
            return View("MyError", new ErrorView("You are not the receiver of that friend request!"));

        friendship.Status = FriendshipStatus.Accepted;
        friendship.StartDate = DateTime.Now;
        _db.SaveChanges();

        // Redirect to the target user's profile.
        int profile_id = _db.Profiles.First(p => p.UserId == friendship.User1Id).Id;
        return RedirectToAction("Index", "Profile", new { id = profile_id });
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