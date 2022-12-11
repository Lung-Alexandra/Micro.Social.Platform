using System.Diagnostics;
using MicroSocialPlatform.Data;
using MicroSocialPlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MicroSocialPlatform.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<AppUser> _userManager;

    // Use dependency injection to get the the application database context and the user manager.
    public HomeController(ApplicationDbContext db, UserManager<AppUser> userManager)
    {
        _userManager = userManager;
        _db = db;
    }

    // The main page.
    public IActionResult Index()
    {
        var userId = _userManager.GetUserId(User);

        // Check if the user has created his profile.
        if (userId != null)
        {
            var user = _db.Users.Find(userId);
            if (user == null) return View("Error");
            // If the user does not have a profile, redirect to the profile creation.
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}