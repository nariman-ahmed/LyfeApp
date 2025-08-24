using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LyfeApp.Data.Services;
using Microsoft.AspNetCore.Mvc;
using LyfeApp.Data.DTO.Friends;
using LyfeApp.Data.Helpers.Constants;

namespace LyfeApp.Controllers
{
    public class FriendsController : BaseController
    {
        private readonly IFriendsService _friendsService;
        public FriendsController(IFriendsService friendsService)
        {
            _friendsService = friendsService;
        }

        public async Task<IActionResult> Index()
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null)
            {
                // If the user is not logged in, redirect to the login page or handle accordingly
                return RedirectToLogin();
            }

            var friendshipDto = new FriendshipDto
            {
                FriendRequestsSent = await _friendsService.GetSentFriendRequestsAsync(loggedInUserId.Value),
                FriendRequestsReceived = await _friendsService.GetReceivedFriendRequestsAsync(loggedInUserId.Value),
                Friends = await _friendsService.GetAllFriendsAsync(loggedInUserId.Value)
            };
            return View(friendshipDto);
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

        [HttpPost]
        public async Task<IActionResult> UpdateFriendRequest(int requestId, string status)
        {
            await _friendsService.UpdateFriendRequestAsync(requestId, status);

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> RemoveFriend(int friendshipId)
        {
            await _friendsService.RemoveFriendAsync(friendshipId);

            return RedirectToAction("Index");
        }
    }
}