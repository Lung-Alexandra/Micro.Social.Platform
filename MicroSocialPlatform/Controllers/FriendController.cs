using Microsoft.AspNetCore.Authorization;
using MicroSocialPlatform.Models;
using MicroSocialPlatform.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace MicroSocialPlatform.Controllers;

public class FriendController: Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<AppUser> _userManager;

    public FriendController(ApplicationDbContext db, UserManager<AppUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }
    // Shows all friends for user id.
    [HttpGet]
    public IActionResult Index(int id)
    {
        List <Friend> friend;
        var userId = _userManager.GetUserId(User);
      try
        {
            friend = _db.Friends
                .Include(p=>p.User1Id)
                .First(x => x.User1Id == userId);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("does not exist!"));
        }
    }
    
}