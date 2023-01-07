using MicroSocialPlatform.Data;
using Microsoft.AspNetCore.Identity;
using MicroSocialPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace MicroSocialPlatform.Controllers;

// This class manages functionality related to group memberships.
public class GroupMembershipController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<AppUser> _userManager;

    public GroupMembershipController(ApplicationDbContext db, UserManager<AppUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPost]
    // This method creates a new membership of the current user to the given group id.
    // The membership will have a pending status until it is accepted by a group admin.
    public IActionResult New(int id)
    {
        Group group;
        // Get the group from the database with the corresponding id.
        try
        {
            group = _db.Groups.Include(g => g.Memberships).First(g => g.Id == id);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("The group does not exist!"));
        }

        string myId = _userManager.GetUserId(User);
        // First check if there isn't another membership that belongs to this user..
        if (group.Memberships.Any(m => m.UserId == myId))
        {
            return View("MyError", new ErrorView("The current user already has a membership to that group!"));
        }

        // Else create the membership.
        GroupMembership membership = new GroupMembership();
        membership.UserId = myId;
        membership.GroupId = id;
        membership.Status = MembershipStatus.Pending;

        _db.GroupMemberships.Add(membership);
        _db.SaveChanges();

        // Redirect to the group page.
        return RedirectToAction("Index", "Group", new { id });
    }
}