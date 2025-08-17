using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LyfeApp.Data.Models
{
    public class UserModel : IdentityUser<int>  //we replaced the basic IdentityUser class with this as we wanted to extend it
    {
        // public int Id { get; set; }
        /*el primary key type hena is int (id) bas fe identity user its string
        and also identity user already has id attribute fa fee repetition. to solve this, instead of deleting 
        this attribute and having to delete any reference in the whole app of it, we can just change
        the primary key type of identity user to int but we would also have to set it to int
        for the class user roles. so we do that in program.cs and appdbcontext*/

        public string FullName { get; set; }

        public string? ProfilePicUrl { get; set; }
        public string? Bio { get; set; }
        public bool IsDeleted { get; set; }


        public ICollection<PostModel> Posts { get; set; } = new List<PostModel>();
        public ICollection<LikeModel> Likes { get; set; } = new List<LikeModel>();
        public ICollection<CommentModel> Comments { get; set; } = new List<CommentModel>();
        public ICollection<FavoriteModel> Favorites { get; set; } = new List<FavoriteModel>();

    }
}