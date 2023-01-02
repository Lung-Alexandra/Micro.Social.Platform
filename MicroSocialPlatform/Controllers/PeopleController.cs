using MicroSocialPlatform.Data;
using MicroSocialPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MicroSocialPlatform.Controllers;

// This class is responsible for the page "People" in the app.
// It allows the user to see his friends and to search for new
// people.
public class PeopleController : Controller
{
    private readonly ApplicationDbContext _db;

    public PeopleController(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        // Get the search parameter from the request.
        var search = HttpContext.Request.Query["search"].ToString();

        // Get all users.
        IQueryable<AppUser> users = _db.Users.Include(u => u.UserProfile);

        // If the search parameter exists, filter users by their name.
        if (search != null)
        {
            users = users.Where(u => u.UserName.Contains(search));
        }

        // Return the list of users.
        PeopleView people = new PeopleView(users.ToList());
        return View(people);
    }
}