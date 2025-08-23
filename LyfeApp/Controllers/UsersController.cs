using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LyfeApp.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LyfeApp.Data.DTO.Users;

namespace LyfeApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _usersService;
        public UsersController(IUserService usersService)
        {
            _usersService = usersService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int userId)
        {
            var userPosts = await _usersService.GetUserPostsAsync(userId);

            var userDetailsDto = new UserProfileDetailsDto
            {
                User = await _usersService.GetUserAsync(userId),
                Posts = userPosts
            };

            return View(userDetailsDto);
        }
    }
}