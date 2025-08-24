using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LyfeApp.Data.Models;

namespace LyfeApp.Data.DTO.Friends
{
    public class FriendshipDto   //el dto da haysheel kol haga hatezhar fel friend index view
    {
        public List<FriendRequest> FriendRequestsSent { get; set; } = new List<FriendRequest>();
        public List<FriendRequest> FriendRequestsReceived { get; set; } = new List<FriendRequest>();
        public List<Friendship> Friends { get; set; } = new List<Friendship>();
    }
}