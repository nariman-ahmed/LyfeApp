using System.Diagnostics;
using LyfeApp.Data;
using LyfeApp.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LyfeApp.Data.DTO.Home;
using LyfeApp.Data.Services;



namespace LyfeApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostsService _postService;
        private readonly IFilesService _filesService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IPostsService postService, IFilesService filesService)
        {
            _postService = postService;
            _filesService = filesService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            int loggedInUserId = 1;
            var allPosts = await _postService.GetAllPostsAsync(loggedInUserId);

            return View(allPosts);    //sent to index.cshtml zay el function name
        }

        public async Task<IActionResult> PostDetails(int postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);

            return View(post);   //what this means is that in the Views, in the home folder you must have a view with the same name as the method, and thats where the post will be sent
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatedPostDto createdPost)
        {
            int loggedInUser = 1;

            var imageUploadPath = await _filesService.UploadImageAsync(createdPost.Image, ImageFileType.PostImage);

            var newPost = new PostModel
            {
                Content = createdPost.Content,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                ImageUrl = imageUploadPath,
                NumReports = 0,
                UserId = loggedInUser
            };

            await _postService.CreatePostAsync(newPost);

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
