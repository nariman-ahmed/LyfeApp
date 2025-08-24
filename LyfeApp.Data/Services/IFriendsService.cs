using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LyfeApp.Data.Models;
using LyfeApp.Data.DTO.Users;

namespace LyfeApp.Data.Services
{
    public interface IFriendsService
    {
        Task SendFriendRequestAsync(int senderId, int receiverId);
        Task UpdateFriendRequestAsync(int requestId, string status);
        Task RemoveFriendAsync(int friendshipId);

        Task<List<CountOfUserFriendsDto>> GetSuggestedFriendsAsync(int userId);
    }
}