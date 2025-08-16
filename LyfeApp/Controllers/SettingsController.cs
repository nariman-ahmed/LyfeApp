using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LyfeApp.Data.Services;
using LyfeApp.Data.Models;
using LyfeApp.Data.DTO.Settings;

namespace LyfeApp.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IUserService _usersService;
        private readonly IFilesService _filesService;
        public SettingsController(IUserService usersService, IFilesService filesService)
        {
            _usersService = usersService;
            _filesService = filesService;
        }

        public async Task<IActionResult> Index()
        {
            var loggedInUserId = 1;
            var currentUser = await _usersService.GetUserAsync(loggedInUserId);
            return View(currentUser);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfilePic(UpdateProfilePicDto updateDto)
        {
            var loggedInUserId = 1;

            var uploadedImageUrl = await _filesService.UploadImageAsync(updateDto.NewProfilePic, ImageFileType.ProfilePicture);

            await _usersService.UpdateUserProfilePic(loggedInUserId, uploadedImageUrl);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto profileDto)
        {
            // int loggedInUserId = 1;

            // await _usersService.UpdateProfile(loggedInUserId, profileDto.fullname, profileDto.username, profileDto.email, profileDto.bio);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordDto passwordDto)
        {
            return RedirectToAction("Index");
            
        }

    }
}