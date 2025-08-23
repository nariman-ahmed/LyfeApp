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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace LyfeApp.Controllers
{
    [Authorize]
    public class SettingsController : BaseController
    {
        private readonly IUserService _usersService;
        private readonly IFilesService _filesService;
        private readonly UserManager<UserModel> _userManager;
        public SettingsController(IUserService usersService, IFilesService filesService, UserManager<UserModel> userManager)
        {
            _usersService = usersService;
            _filesService = filesService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            //var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // name identifier gets the user id
            //when someone logs in, asp automatically created the object "User"
            //containing their claims and identity information which we can use
            //throughout the whole codebase.
            
            var loggedInUser = await _userManager.GetUserAsync(User);
            return View(loggedInUser);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfilePic(UpdateProfilePicDto updateDto)
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null)
            {
                // If the user is not logged in, redirect to the login page or handle accordingly
                return RedirectToLogin();
            }

            var uploadedImageUrl = await _filesService.UploadImageAsync(updateDto.NewProfilePic, ImageFileType.ProfilePicture);

            await _usersService.UpdateUserProfilePic(loggedInUserId.Value, uploadedImageUrl);

            return RedirectToAction("Index");
        }

    }
}