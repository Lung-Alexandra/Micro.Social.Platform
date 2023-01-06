using MicroSocialPlatform.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using MicroSocialPlatform.Models;

namespace MicroSocialPlatform.Controllers;

public class GroupController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<AppUser> _userManager;

    public GroupController(ApplicationDbContext db, UserManager<AppUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    // Show all created groups.
    public IActionResult All()
    {
        // Get the list of groups.
        List<Group> groups = _db.Groups.ToList();
        return View(new GroupsView(groups));
    }

    // Show the group page given by id.
    public IActionResult Index(int id)
    {
        // Get the group by id from the database.
        Group group;
        try
        {
            group = _db.Groups.First(g => g.Id == id);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("That group does not exist!"));
        }

        return View(group);
    }

    [Authorize(Roles = "User,Admin")]
    [HttpGet]
    // Only users and admins can create groups.
    public IActionResult New()
    {
        // Show the page to create a new group .
        return View();
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPost]
    // Only users and admins can create groups.
    // Create a new group using the post request data.
    public IActionResult New(Group newGroup)
    {
        newGroup.CreationTime = DateTime.Now;
        // Check if the model state is valid.
        if (ModelState.IsValid)
        {
            newGroup.UserId = _userManager.GetUserId(User);
            _db.Groups.Add(newGroup);
            _db.SaveChanges();
            return RedirectToAction("Index", new { id = newGroup.Id });
        }

        // The model is invalid, show errors.
        return View(newGroup);
    }
}