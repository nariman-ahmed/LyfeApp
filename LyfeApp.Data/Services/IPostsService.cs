using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using LyfeApp.Data.Models;

namespace LyfeApp.Data.Services
{
    public interface IPostsService
    {
        Task<List<PostModel>> GetAllPostsAsync();

        Task<PostModel> CreatePostAsync(PostModel post, IFormFile image);

        Task DeletePostAsync(int postId);

        Task CreateCommentAsync(CommentModel comment);
        Task DeleteCommentAsync(int commentId);

        Task TogglePostLikeAsync(int postId, int userId);
        Task TogglePostFavoriteAsync(int postId, int userId);

    }
}