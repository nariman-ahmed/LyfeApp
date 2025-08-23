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
using Microsoft.IdentityModel.Tokens;

namespace LyfeApp.Controllers
{
    public class StoriesController : Controller
    {
        private readonly IFilesService _filesService;
        private readonly IStoriesService _storiesService;

        public StoriesController(IFilesService filesService, IStoriesService storiesService)
        {
            _filesService = filesService;
            _storiesService = storiesService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateStoryAsync(CreatedStoryDto createdStory)
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

            await _storiesService.CreateStoryAsync(story);

            return RedirectToAction("Index", "Home");
        }
    }
}