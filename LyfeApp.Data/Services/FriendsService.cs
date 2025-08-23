using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LyfeApp.Data.Helpers.Constants;
using LyfeApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LyfeApp.Data.Services
{
    public class FriendsService : IFriendsService
    {
        private readonly ApplicationDbContext _context;
        public FriendsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task SendFriendRequestAsync(int senderId, int receiverId)
        {
            //we a friend request is made, we just want to add its record to the db 
            var newRequest = new FriendRequest
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Status = FriendshipStatus.Pending,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };
            await _context.FriendRequests.AddAsync(newRequest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFriendRequestAsync(int requestId, string status)
        {
            var requestDb = await _context.FriendRequests.FirstOrDefaultAsync(fr => fr.Id == requestId);

            if (requestDb != null)
            {
                requestDb.Status = status;
                requestDb.DateUpdated = DateTime.UtcNow;

                _context.FriendRequests.Update(requestDb);
                await _context.SaveChangesAsync();

                if (status == FriendshipStatus.Accepted)
                {
                    //after accepting, this means they became friends. so we need to add them to the friendship table
                    var newFriendship = new Friendship
                    {
                        SenderId = requestDb.SenderId,
                        ReceiverId = requestDb.ReceiverId,
                        DateCreated = DateTime.UtcNow
                    };
                    await _context.Friendships.AddAsync(newFriendship);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task RemoveFriendAsync(int friendshipId)
        {
            var friendshipDb = await _context.Friendships.FirstOrDefaultAsync(f => f.Id == friendshipId);

            if (friendshipDb != null)
            {
                _context.Friendships.Remove(friendshipDb);
                await _context.SaveChangesAsync();
            }
        }
        

    }
}