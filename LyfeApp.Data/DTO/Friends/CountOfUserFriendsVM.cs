using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyfeApp.Data.DTO.Friends
{
    public class CountOfUserFriendsVM
    {
        public int UserId { get; set; }
        public string? FullName { get; set; }
        public string? ProfilePicUrl { get; set; }
        public int FriendsCount { get; set; }
        public string FriendsCountDisplay => 
            FriendsCount == 0 ? "No followers" :
            FriendsCount == 1 ? "1 follower" : 
            $"{FriendsCount} followers";
    }
}