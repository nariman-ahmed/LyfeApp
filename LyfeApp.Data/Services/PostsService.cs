using LyfeApp.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyfeApp.Data.Services
{
    public class PostsService : IPostsService
    {
        private readonly ApplicationDbContext _context;

        public PostsService(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<List<PostModel>> GetAllPostsAsync()
        {
            var allPosts = await _context.Posts
                .Where(n => n.NumReports < 5 && !n.IsDeleted)
                .Include(n => n.User)
                .Include(n => n.Likes)
                .Include(n => n.Favorites)
                .Include(n => n.Comments).ThenInclude(n => n.User)
                .OrderByDescending(n => n.DateCreated)
                .ToListAsync();

            return allPosts;
        }

        public async Task CreateCommentAsync(CommentModel comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<PostModel> CreatePostAsync(PostModel post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return post;
        }

        public async Task DeletePostAsync(int postId)
        {
            var postDb = await _context.Posts.FirstOrDefaultAsync(n => n.Id == postId);

            if (postDb != null)
            {
                //_context.Posts.Remove(postDb);
                postDb.IsDeleted = true;
                _context.Posts.Update(postDb);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            var commentToBeDeleted = _context.Comments.FirstOrDefault(n => n.Id == commentId);

            if(commentToBeDeleted != null)
            {
                _context.Comments.Remove(commentToBeDeleted);
                await _context.SaveChangesAsync();
            }
        }

        public async Task TogglePostFavoriteAsync(int postId, int userId)
        {
            //check if user has already favorited the post
            var favorite = await _context.Favorites
                .Where(l => l.PostId == postId && l.UserId == userId)
                .FirstOrDefaultAsync();

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
            else
            {
                var newFavorite = new FavoriteModel()
                {
                    PostId = postId,
                    UserId = userId
                };
                await _context.Favorites.AddAsync(newFavorite);
                await _context.SaveChangesAsync();
            }
        }

        public async Task TogglePostLikeAsync(int postId, int userId)
        {
            //check if user has already liked the post
            var like = await _context.Likes
                .Where(l => l.PostId == postId && l.UserId == userId)
                .FirstOrDefaultAsync();

            if (like != null)
            {
                _context.Likes.Remove(like);
                await _context.SaveChangesAsync();
            }
            else
            {
                var newLike = new LikeModel()
                {
                    PostId = postId,
                    UserId = userId
                };
                await _context.Likes.AddAsync(newLike);
                await _context.SaveChangesAsync();
            }
        }
    }
}