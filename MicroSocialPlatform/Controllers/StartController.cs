using System.Diagnostics;
using MicroSocialPlatform.Data;
using MicroSocialPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace MicroSocialPlatform.Controllers;

public class StartController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;
    public StartController(SignInManager<AppUser> signInManager)
    {
        _signInManager= signInManager;
    }

    // The start page.
    public IActionResult Index()
    {
        // If the user is signed in, redirect to the home page.
        if (_signInManager.IsSignedIn(User))
        {
            return RedirectToRoute("home");
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}