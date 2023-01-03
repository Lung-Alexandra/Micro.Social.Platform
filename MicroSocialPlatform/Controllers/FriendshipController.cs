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
        string myId = _userManager.GetUserId(User);

        // Get the friendship by id.
        Friendship friendship;
        try
        {
            friendship = _db.Friendships.First(u => u.Id == id);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("The friend request does not exist!"));
        }

        // Check if the current user is the received of the friend request.
        if (friendship.User2Id != myId)
            return View("MyError", new ErrorView("You are not the receiver of that friend request!"));

        // Check if the friend request was already accepted.
        if (friendship.Status == FriendshipStatus.Accepted)
        {
            return View("MyError", new ErrorView("The friend request was already accepted!"));
        }

        friendship.Status = FriendshipStatus.Accepted;
        friendship.StartDate = DateTime.Now;
        _db.SaveChanges();

        // Redirect to the target user's profile.
        int profileId = _db.Profiles.First(p => p.UserId == friendship.User1Id).Id;
        return RedirectToAction("Index", "Profile", new { id = profileId });
    }

    // Only user and admins can delete friendships.
    [Authorize(Roles = "User,Admin")]
    [HttpPost]
    // Delete a friendship given by id.
    // A friendship can be deleted by either the sender or the receiver
    // of the friendship. The friendship does not need to be accepted for it 
    // to be deleted.
    public IActionResult Delete(int id)
    {
        // First find the friendship by id.
        Friendship friendship;
        try
        {
            // Also include users to redirect to the corresponding profile.
            friendship = _db.Friendships
                .Include(f => f.User1)
                .ThenInclude(u1 => u1.UserProfile)
                .Include(f => f.User2)
                .ThenInclude(u2 => u2.UserProfile)
                .First(f => f.Id == id);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("That friendship does not exist!"));
        }

        string myId = _userManager.GetUserId(User);
        bool sender = myId == friendship.User1Id;
        bool receiver = myId == friendship.User2Id;

        // Then check if the user is either the sender or the receiver of the friendship, or an admin.
        if (sender || receiver || User.IsInRole("Admin"))
        {
            _db.Friendships.Remove(friendship);
            _db.SaveChanges();

            // If the user is an admin redirect to the home page.
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            // Get the profile id of the other user.
            int otherId = sender ? friendship.User2.UserProfile.Id : friendship.User1.UserProfile.Id;
            return RedirectToAction("Index", "Profile", new { id = otherId });
        }

        return View("MyError", new ErrorView("You are not the sender or the receiver of that friendship!"));
    }
}