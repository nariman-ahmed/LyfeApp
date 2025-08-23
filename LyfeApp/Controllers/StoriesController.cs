using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LyfeApp.Data;
using LyfeApp.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LyfeApp.Data.DTO.Stories;
using System.Security.Claims;
using LyfeApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LyfeApp.Controllers
{
    public class StoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFilesService _filesService;

        public StoriesController(ApplicationDbContext context, IFilesService filesService)
        {
            _context = context;
            _filesService = filesService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var allStories = await _context.Stories
            .Include(s => s.User)
            .OrderByDescending(s => s.DateCreated)
            .ToListAsync();
            return View(allStories);
        }

        public async Task<IActionResult> CreateStory(CreatedStoryDto createdStory)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedInUserId))
            {
                // If the user is not logged in, redirect to the login page or handle accordingly
                return RedirectToAction("Login", "Authentication");
            }

            var imageUploadPath = await _filesService.UploadImageAsync(createdStory.Image, ImageFileType.StoryImage);

            var story = new StoryModel
            {
                // Map properties from createdStory to story
                ImageUrl = imageUploadPath,
                DateCreated = DateTime.UtcNow,
                IsDeleted = false,
                UserId = int.Parse(loggedInUserId)
            };

            await _context.Stories.AddAsync(story);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}