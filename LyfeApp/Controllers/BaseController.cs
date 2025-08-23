using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LyfeApp.Controllers
{
    public abstract class BaseController : Controller
    {
        protected int? GetUserId()
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedInUserId))
            {
                // If the user is not logged in, redirect to the login page or handle accordingly
                return null;
            }
            return int.Parse(loggedInUserId);
        }

        protected IActionResult RedirectToLogin()
        {
            return RedirectToAction("Login", "Authentication");
        }

    }
}