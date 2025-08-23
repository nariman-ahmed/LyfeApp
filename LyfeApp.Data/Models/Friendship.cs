using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyfeApp.Data.Models
{
    public class Friendship
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public int SenderId { get; set; }
        public virtual UserModel Sender { get; set; }
        public int ReceiverId { get; set; }
        public virtual UserModel Receiver { get; set; }

        //we use the keyword virtual to enable lazy loading (an EF core attribute)
        //which means these navigation properties are only loaded when we actually access them
    }
}