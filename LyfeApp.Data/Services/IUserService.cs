using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LyfeApp.Data.Models;

namespace LyfeApp.Data.Services
{
    public interface IUserService
    {
        Task<UserModel> GetUserAsync(int loggedInUserId);

        Task UpdateUserProfilePic(int loggedInUserId, string newImageUrl);

        Task<List<PostModel>> GetUserPostsAsync(int userId);

    }
}