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
        private readonly IFriendsService _friendsService;
        
        public UsersController(IUserService usersService, IFriendsService friendsService)
        {
            _usersService = usersService;
            _friendsService = friendsService;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int userId)
        {
            var userPosts = await _usersService.GetUserPostsAsync(userId);
            var userFriends = await _friendsService.GetAllFriendsAsync(userId);

            var userDetailsDto = new UserProfileDetailsDto
            {
                User = await _usersService.GetUserAsync(userId),
                Posts = userPosts,
                Friends = userFriends
            };

            return View(userDetailsDto);
        }
    }
}