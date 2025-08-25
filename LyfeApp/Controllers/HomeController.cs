using System.Diagnostics;
using LyfeApp.Data;
using LyfeApp.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LyfeApp.Data.DTO.Home;
using LyfeApp.Data.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace LyfeApp.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IPostsService _postService;
        private readonly IFilesService _filesService;

        public HomeController(IPostsService postService, IFilesService filesService)
        {
            _postService = postService;
            _filesService = filesService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null)
            {
                // If the user is not logged in, redirect to the login page or handle accordingly
                return RedirectToLogin();
            }

            var allPosts = await _postService.GetAllPostsAsync(loggedInUserId.Value);

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
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null)
            {
                // If the user is not logged in, redirect to the login page or handle accordingly
                return RedirectToLogin();
            }

            var imageUploadPath = await _filesService.UploadImageAsync(createdPost.Image, ImageFileType.PostImage);

            var newPost = new PostModel
            {
                Content = createdPost.Content,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                ImageUrl = imageUploadPath,
                NumReports = 0,
                UserId = loggedInUserId.Value
            };

            await _postService.CreatePostAsync(newPost);

            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TogglePostLike(PostLikesDto postLikesDto)
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null)
            {
                // If the user is not logged in, redirect to the login page or handle accordingly
                return RedirectToLogin();
            }

            await _postService.TogglePostLikeAsync(postLikesDto.PostId, loggedInUserId.Value);

            var post = await _postService.GetPostByIdAsync(postLikesDto.PostId);

            return PartialView("Home/_Post", post);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateNewCommentDto commentDto)
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null)
            {
                // If the user is not logged in, redirect to the login page or handle accordingly
                return RedirectToLogin();
            }   

            var newComment = new CommentModel()
            {
                Content = commentDto.Content,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                PostId = commentDto.PostId,
                UserId = loggedInUserId.Value
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
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null)
            {
                // If the user is not logged in, redirect to the login page or handle accordingly
                return RedirectToLogin();
            }

            await _postService.TogglePostFavoriteAsync(postFavDto.PostId, loggedInUserId.Value);

            var post = await _postService.GetPostByIdAsync(postFavDto.PostId);

            return PartialView("Home/_Post", post);
        }

        [HttpPost]
        public async Task<IActionResult> TogglePostPrivacy(PostPrivacyDto postPrivacyDto)
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null)
            {
                // If the user is not logged in, redirect to the login page or handle accordingly
                return RedirectToLogin();
            }

            await _postService.TogglePostPrivacyAsync(postPrivacyDto.PostId, loggedInUserId.Value);

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
