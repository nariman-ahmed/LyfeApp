using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LyfeApp.ViewComponents
{
    public class SuggestedFriendsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // // Get the current user's friends
            // var friends = await _friendsService.GetFriendsAsync(User.GetUserId());

            // // Get suggested friends (not friends yet)
            // var suggestedFriends = await _friendsService.GetSuggestedFriendsAsync(User.GetUserId());

            // // Pass the data to the view
            // return View(new SuggestedFriendsViewModel
            // {
            //     Friends = friends,
            //     SuggestedFriends = suggestedFriends
            // });
            return View();    //el view el hayeb2a called lazem yeb2a fe folder Views/Shared/Components/SuggestedFriends(this filename - controller) bezabt.
        }
    }
}