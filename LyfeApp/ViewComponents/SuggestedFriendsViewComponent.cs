using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LyfeApp.Data.Services;
using Microsoft.AspNetCore.Mvc;
using LyfeApp.Data.DTO.Users;
using LyfeApp.Data.DTO.Friends;

namespace LyfeApp.ViewComponents
{
    public class SuggestedFriendsViewComponent : ViewComponent
    {
        private readonly IFriendsService _friendsService;

        public SuggestedFriendsViewComponent(IFriendsService friendsService)
        {
            _friendsService = friendsService;   
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var loggedInUserId = ((ClaimsPrincipal)User).FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(loggedInUserId);
            var suggestedFriends = await _friendsService.GetSuggestedFriendsAsync(userId);

            var suggestedFriendsVM = suggestedFriends.Select(sf => new CountOfUserFriendsVM
            {
                UserId = sf.User.Id,
                FullName = sf.User.FullName,
                ProfilePicUrl = sf.User.ProfilePicUrl,
                FriendsCount = sf.FriendsCount
            }).ToList();

            return View(suggestedFriendsVM);  //el VM howa el hayetbe3et lel view component
        }
    }
}