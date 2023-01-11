using MicroSocialPlatform.Data;
using Microsoft.AspNetCore.Authorization;
using MicroSocialPlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MicroSocialPlatform.Controllers;

using Microsoft.AspNetCore.Mvc;

public class FriendController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<AppUser> _userManager;

    public FriendController(ApplicationDbContext db, UserManager<AppUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    [Authorize(Roles = "User,Admin")]
    public IActionResult Index()
    {
        // Get the search parameter from the request.
        var search = HttpContext.Request.Query["search"].ToString();
        string myId = _userManager.GetUserId(User);
        // AppUser current_user;
        // try
        // {
        //     current_user = _db.Users.First(u => u.Id == myId);
        // }
        // catch (InvalidOperationException)
        // {
        //     return View("MyError", new ErrorView("The user does not exist."));
        // }


        List<AppUser> friends = new List<AppUser>();
        var lista_useri = _db.Friendships
            .Include(f=>f.User1)
            .ThenInclude(u=>u.UserProfile)
            .Include(f => f.User2)
            .ThenInclude(u=>u.UserProfile)
            .Where(f => (f.User1Id == myId || f.User2Id==myId) && f.Status == FriendshipStatus.Accepted);
        foreach (var friendship in lista_useri)
        {
            if(friendship.User1Id == myId)
                friends.Add(friendship.User2);
            else friends.Add(friendship.User1);
        }

        
        
        // If the search parameter exists, filter users by their name.
        if (search != null)
        {
            friends = friends.Where(u => u.UserName.Contains(search)).ToList();
        }
        // //
        // // // Return the list of users.
        FriendsView view = new FriendsView(friends);
        return View(view);
    }
}