using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LyfeApp.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LyfeApp.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly IPostsService _postsService;
        public FavoritesController(IPostsService postsService)
        {
            _postsService = postsService;
        }

        public async Task<IActionResult> Index()
        {
            /*kol controller lee view. lazem gowa el View folder a3mel folder gdeed esmo Favorites
            wa7ot fee file index 3shan da el hayeb2a rendered mel funtion di zay ma fee folder esmo Home
            fee file index.cshtml how el ehna hateen fee layout el home!*/
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedInUserId))
            {
                // If the user is not logged in, redirect to the login page or handle accordingly
                return RedirectToAction("Login", "Authentication");
            }
            var allFavPosts = await _postsService.GetAllFavoritedPostsAsync(int.Parse(loggedInUserId));

            return View(allFavPosts);
        }


    }
}