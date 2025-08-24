using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LyfeApp.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace LyfeApp.Controllers
{
    public class FriendsController : BaseController
    {
        private readonly IFriendsService _friendsService;
        public FriendsController(IFriendsService friendsService)
        {
            _friendsService = friendsService;
        }

        [HttpPost]
        public async Task<IActionResult> SendFriendRequest(int receiverId)
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null)
            {
                // If the user is not logged in, redirect to the login page or handle accordingly
                return RedirectToLogin();
            }

            await _friendsService.SendFriendRequestAsync(loggedInUserId.Value, receiverId);
            return RedirectToAction("Index", "Home");
        }
    }
}