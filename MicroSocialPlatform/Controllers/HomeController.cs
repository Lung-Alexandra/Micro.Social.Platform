using System.Diagnostics;
using MicroSocialPlatform.Data;
using MicroSocialPlatform.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroSocialPlatform.Controllers;

public class HomeController : Controller
{
    // The main page.
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}