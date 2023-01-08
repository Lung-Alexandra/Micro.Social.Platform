using MicroSocialPlatform.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using MicroSocialPlatform.Models;
using Microsoft.EntityFrameworkCore;

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
            group = _db.Groups
                .Include(g => g.Memberships)
                .ThenInclude(m => m.User)
                .First(g => g.Id == id);
        }
        catch (InvalidOperationException)
        {
            return View("MyError", new ErrorView("That group does not exist!"));
        }

        string myId = _userManager.GetUserId(User);
        GroupMembership myMembership = group.Memberships.FirstOrDefault(m => m.UserId == myId);
        group.userMembership = myMembership;

        bool groupAdmin = myMembership != null && myMembership.Status == MembershipStatus.Admin;
        // The user can modify the group if it is a group admin or an admin.
        bool canModify = groupAdmin || User.IsInRole("Admin");

        foreach (var membership in group.Memberships)
        {
            membership.userCanModify = canModify;
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
    // Creating a group will make the current user join the group as an admin.
    public IActionResult New(Group newGroup)
    {
        newGroup.CreationTime = DateTime.Now;
        // Check if the model state is valid.
        if (ModelState.IsValid)
        {
            // Add the group.
            newGroup.UserId = _userManager.GetUserId(User);
            _db.Groups.Add(newGroup);
            _db.SaveChanges();

            // Then make the current user the admin of the group.
            GroupMembership membership = new GroupMembership();
            membership.UserId = _userManager.GetUserId(User);
            membership.JoinDate = DateTime.Now;
            membership.Status = MembershipStatus.Admin;
            membership.GroupId = newGroup.Id;

            _db.GroupMemberships.Add(membership);
            _db.SaveChanges();
            return RedirectToAction("Index", new { id = newGroup.Id });
        }

        // The model is invalid, show errors.
        return View(newGroup);
    }

    [Authorize(Roles = "User,Admin")]
    [HttpGet]
    // Edit a group given by the id.
    public IActionResult Edit(int id)
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

        // Get the membership of the current user to check if it has the permission to edit.
        GroupMembership? membership = group.Memberships.FirstOrDefault(m => m.UserId == _userManager.GetUserId(User));

        // Check if the user is a group admin.
        bool groupAdmin = membership != null && membership.Status == MembershipStatus.Admin;

        // If the user is a group admin or an admin, it can edit the group. 
        if (groupAdmin || User.IsInRole("Admin"))
        {
            return View(group);
        }

        return View("MyError", new ErrorView("Only group admins can edit the group!"));
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPost]
    // Edit the group given by the id.
    public IActionResult Edit(int id, Group newGroup)
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

        // Get the membership of the current user to check if it has the permission to edit.
        GroupMembership? membership = group.Memberships.FirstOrDefault(m => m.UserId == _userManager.GetUserId(User));

        // Check if the user is a group admin.
        bool groupAdmin = membership != null && membership.Status == MembershipStatus.Admin;

        // If there are no model state errors.
        if (ModelState.IsValid)
        {
            // Check if user is an admin or a group admin.
            if (groupAdmin || User.IsInRole("Admin"))
            {
                group.Name = newGroup.Name;
                group.Description = newGroup.Description;
                _db.SaveChanges();
                return RedirectToAction("Index", new { id });
            }

            return View("MyError", new ErrorView("Only group admins can edit the group!"));
        }

        // Then show errors on page.
        return View(group);
    }
}