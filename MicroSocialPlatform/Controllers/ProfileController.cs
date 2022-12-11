using Microsoft.AspNetCore.Authorization;

namespace MicroSocialPlatform.Controllers;
using Microsoft.AspNetCore.Mvc;

public class ProfileController : Controller
{
    // Shows the profile of the user.
    [Authorize(Roles="User,Admin")]
    public IActionResult Index()
    {
        return View();
    }
    
    // Edits the profile of the user.
    [Authorize(Roles="User,Admin")]
    public IActionResult Edit()
    {
        return View();
    }
}