using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LyfeApp.Data.Models
{
    public class PostModel
    {
        [Key]
        public int Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public int NumReports { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        //forign key to UserModel
        public int UserId { get; set; }

        //navigation property
        public UserModel User { get; set; }

        //a post can have many likes and many comments
        public ICollection<LikeModel> Likes { get; set; } = new List<LikeModel>();
        public ICollection<CommentModel> Comments { get; set; } = new List<CommentModel>();
        public ICollection<FavoriteModel> Favorites { get; set; } = new List<FavoriteModel>();


    }
}