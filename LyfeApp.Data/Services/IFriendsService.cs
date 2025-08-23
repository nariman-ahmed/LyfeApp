using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyfeApp.Data.Services
{
    public interface IFriendsService
    {
        Task SendFriendRequestAsync(int senderId, int receiverId);
        Task UpdateFriendRequestAsync(int requestId, string status);
        Task RemoveFriendAsync(int friendshipId); 
    }
}