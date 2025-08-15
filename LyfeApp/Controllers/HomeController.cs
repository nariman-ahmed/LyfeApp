using System.Diagnostics;
using LyfeApp.Data;
using LyfeApp.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LyfeApp.Data.DTO.Home;
using LyfeApp.Data.Services;

using System.Windows.Markup;

namespace LyfeApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IPostsService _postService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IPostsService postService)
        {
            _logger = logger;
            _context = context;
            _postService = postService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var allPosts = await _postService.GetAllPostsAsync();

            return View(allPosts);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatedPostDto createdPost)
        {
            int loggedInUser = 1;

            var newPost = new PostModel
            {
                Content = createdPost.Content,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                ImageUrl = "",
                NumReports = 0,
                UserId = loggedInUser
            };

            await _postService.CreatePostAsync(newPost, createdPost.Image);

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> TogglePostLike(PostLikesDto postLikesDto)
        {
            int loggedInUser = 1;

            await _postService.TogglePostLikeAsync(postLikesDto.PostId, loggedInUser);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateNewCommentDto commentDto)
        {
            int loggedInUser = 1;

            var newComment = new CommentModel()
            {
                Content = commentDto.Content,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                PostId = commentDto.PostId,
                UserId = loggedInUser
            };

            await _postService.CreateCommentAsync(newComment);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(DeleteCommentDto comment)
        {
            //hakhod mel ui el comment id
            await _postService.DeleteCommentAsync(comment.CommentId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> TogglePostFavorites(PostFavoritesDto postFavDto)
        {
            int loggedInUser = 1;

            await _postService.TogglePostFavoriteAsync(postFavDto.PostId, loggedInUser);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> PostDelete(DeletePostDto postdto)
        {
            await _postService.DeletePostAsync(postdto.PostId);
            
            return RedirectToAction("Index");
        }
    }
}
