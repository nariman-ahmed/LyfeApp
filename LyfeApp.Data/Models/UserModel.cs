using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LyfeApp.Data.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string? ProfilePicUrl { get; set; }

        public ICollection<PostModel> Posts { get; set; } = new List<PostModel>();
        public ICollection<LikeModel> Likes { get; set; } = new List<LikeModel>();
        public ICollection<CommentModel> Comments { get; set; } = new List<CommentModel>();
        public ICollection<FavoriteModel> Favorites { get; set; } = new List<FavoriteModel>();

    }
}