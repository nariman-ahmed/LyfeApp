using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyfeApp.Data.Models
{
    public class LikeModel
    {
        public int Id { get; set; }

        //2 foreign keys
        public int PostId { get; set; }
        public int UserId { get; set; }

        //navigation property
        public PostModel Post { get; set; }
        public UserModel User { get; set; }

    }
}