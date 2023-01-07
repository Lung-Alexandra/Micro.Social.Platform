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

    [Authorize(Roles = "User,Admin")]
    [HttpPost]
    // Accepts the pending membership given by id.
    public IActionResult Accept(int id)
    {
        GroupMembership membership;
        // Get the group from the database with the corresponding id.
        try
        {
            membership = _db.GroupMemberships
                .Include(m => m.Group)
                .ThenInclude(g => g.Memberships)
                .First(m => m.Id == id);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("The membership does not exist!"));
        }

        // Then check if the membership is pending.
        if (membership.Status != MembershipStatus.Pending)
        {
            return View("MyError", new ErrorView("The membership is not pending!"));
        }

        string myId = _userManager.GetUserId(User);

        // See if the current user is a group admin.
        bool groupAdmin = membership.Group.Memberships.Any(m => m.UserId == myId && m.Status == MembershipStatus.Admin);

        // Only group admins and admins can accept pending memberships.
        if (groupAdmin || User.IsInRole("Admin"))
        {
            membership.Status = MembershipStatus.Member;
            membership.JoinDate = DateTime.Now;
            _db.SaveChanges();
            // Redirect to the group.
            return RedirectToAction("Index", "Group", new { id = membership.GroupId });
        }

        return View("MyError", new ErrorView("Only group admins can accept memberships!"));
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPost]
    // Deletes the membership given by id.
    public IActionResult Delete(int id)
    {
        GroupMembership membership;
        // Get the group from the database with the corresponding id.
        try
        {
            membership = _db.GroupMemberships
                .Include(m => m.Group)
                .ThenInclude(g => g.Memberships)
                .First(m => m.Id == id);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("The membership does not exist!"));
        }

        string myId = _userManager.GetUserId(User);
        bool groupAdmin = membership.Group.Memberships.Any(m => m.UserId == myId && m.Status == MembershipStatus.Admin);
        bool ownsMembership = membership.UserId == myId;

        // The memberships can be deleted by either
        // 1) the user who owns them (he can leave the group)
        // 2) a group admin (he can kick users out).
        // 3) an admin.

        if (ownsMembership || groupAdmin || User.IsInRole("Admin"))
        {
            _db.GroupMemberships.Remove(membership);
            _db.SaveChanges();
            return RedirectToAction("Index", "Group", new { id = membership.GroupId });
        }

        return View("MyError", new ErrorView("Only group admins and owners can delete memberships!"));
    }
}