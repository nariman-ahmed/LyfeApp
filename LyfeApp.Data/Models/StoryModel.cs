using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyfeApp.Data.Models
{
    public class StoryModel
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }

        //forign key to UserModel
        public int UserId { get; set; }

        //navigation property
        public UserModel User { get; set; }
    }
}