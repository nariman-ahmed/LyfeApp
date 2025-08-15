using System.Diagnostics;
using LyfeApp.Data;
using LyfeApp.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LyfeApp.Data.DTO.Home;

using System.Windows.Markup;

namespace LyfeApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var allPosts = await _context.Posts
            .Include(p => p.User)
            .Include(p => p.Likes)
            .OrderByDescending(p => p.DateCreated)
            .ToListAsync();

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

            //check and save image
            if (createdPost.Image != null && createdPost.Image.Length > 0)
            {
                string rootFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                if (createdPost.Image.ContentType.Contains("image"))
                {
                    string rootFolderPathImages = Path.Combine(rootFolderPath, "images/uploaded");
                    Directory.CreateDirectory(rootFolderPathImages);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(createdPost.Image.FileName);
                    string filePath = Path.Combine(rootFolderPathImages, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await createdPost.Image.CopyToAsync(stream);

                    //Set the URL to the newPost object
                    newPost.ImageUrl = "/images/uploaded/" + fileName;
                }
            }

            await _context.Posts.AddAsync(newPost);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> TogglePostLike(PostLikesDto postLikesDto)
        {
            int loggedInUser = 1;

            //check if user had liked the post. find the like with the same post and user id
            var like = await _context.Likes
            .Where(l => l.PostId == postLikesDto.PostId && l.UserId == loggedInUser)
            .FirstOrDefaultAsync();

            if (like != null)  //user already liked so we want to dislike
            {
                _context.Likes.Remove(like);
                await _context.SaveChangesAsync();
            }
            else  //no like (null). add a like
            {
                var newLike = new LikeModel()
                {
                    PostId = postLikesDto.PostId,
                    UserId = loggedInUser
                };

                await _context.Likes.AddAsync(newLike);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // [HttpPost]
        // public async Task<IActionResult> CreateComment(CreateNewCommentDto commentDto)
        // {
        //     int loggedInUser = 1;

        //     var newComment = new CommentModel()
        //     {
                                                                         
        //     }
        // }
    }
}
