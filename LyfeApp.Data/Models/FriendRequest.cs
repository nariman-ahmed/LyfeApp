using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyfeApp.Data.Models
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public int SenderId { get; set; }
        public UserModel Sender { get; set; }
        public int ReceiverId { get; set; }
        public UserModel Receiver { get; set; }
    }
}