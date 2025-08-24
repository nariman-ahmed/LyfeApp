using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LyfeApp.Data.Helpers.Constants;
using LyfeApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using LyfeApp.Data.Services;
using LyfeApp.Data.DTO.Users;


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

                //find requests
                var requests = await _context.FriendRequests
                    .Where(fr => (fr.SenderId == friendshipDb.SenderId && fr.ReceiverId == friendshipDb.ReceiverId) ||
                                  (fr.SenderId == friendshipDb.ReceiverId && fr.ReceiverId == friendshipDb.SenderId))
                    .ToListAsync();

                if (requests.Any())
                {
                    _context.FriendRequests.RemoveRange(requests);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<List<CountOfUserFriendsDto>> GetSuggestedFriendsAsync(int userId)
        {
            //we wanna see the users current friends. friendships that have been made from: 
            //1- either the user sending or accepting a request.
            //and 2- if the user is the sender get the recievers and vice versa
            var existingFriendsIds = await _context.Friendships
            .Where(f => f.SenderId == userId || f.ReceiverId == userId)
            .Select(f => f.SenderId == userId ? f.ReceiverId : f.SenderId)
            .ToListAsync();

            //we will also get the pending requests for that user
            var pendingRequestIds = await _context.FriendRequests
                .Where(n => (n.SenderId == userId || n.ReceiverId == userId) && n.Status == FriendshipStatus.Pending)
                .Select(n => n.SenderId == userId ? n.ReceiverId : n.SenderId)
                .ToListAsync();

            // Now we need to find users who are not in the existing friends list which excludes
            //the current user, the existing friends, and the pending requests
            var suggestedFriends = await _context.Users
            .Where(u => u.Id != userId && !existingFriendsIds.Contains(u.Id) && !pendingRequestIds.Contains(u.Id))
            .Select(u => new CountOfUserFriendsDto
            {
                User = u,
                FriendsCount = _context.Friendships.Count(f => f.SenderId == u.Id || f.ReceiverId == u.Id)
            })
            .Take(5) //limit to 5 suggested friends
            .ToListAsync();

            return suggestedFriends;
        }

        public async Task<List<FriendRequest>> GetSentFriendRequestsAsync(int userId)
        {
            var sentRequests = await _context.FriendRequests
            .Include(f => f.Sender)     //include both the sender and receiver
            .Include(f => f.Receiver)
            .Where(f => f.SenderId == userId && f.Status == FriendshipStatus.Pending)
            //we only want the requests that are still pending
            .ToListAsync();

            return sentRequests;
        }

        public async Task<List<FriendRequest>> GetReceivedFriendRequestsAsync(int userId)
        {
            var receivedRequests = await _context.FriendRequests
            .Include(f => f.Sender)     //include both the sender and receiver
            .Include(f => f.Receiver)
            .Where(f => f.ReceiverId == userId && f.Status == FriendshipStatus.Pending)
            //we only want the requests that are still pending
            .ToListAsync();

            return receivedRequests;
        }

        public async Task<List<Friendship>> GetAllFriendsAsync(int userId)
        {
            var allFriends = await _context.Friendships
            .Where(f => f.SenderId == userId || f.ReceiverId == userId)
            .Include(f => f.Sender)
            .Include(f => f.Receiver)
            .ToListAsync();

            return allFriends;
        }

    }
}
        
