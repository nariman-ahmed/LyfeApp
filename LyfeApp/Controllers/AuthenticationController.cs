using LyfeApp.Data.Helpers.Constants;
using LyfeApp.Data.Models;
using LyfeApp.Data.DTO.Authentication;
using LyfeApp.Data.DTO.Settings;
using LyfeApp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace CircleApp.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        public AuthenticationController(UserManager<UserModel> userManager,
            SignInManager<UserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if(!ModelState.IsValid)
                return View(loginDto);

            var existingUser = await _userManager.FindByEmailAsync(loginDto.Email);
            if(existingUser == null)
            {
                ModelState.AddModelError("", "Invalid email or password. Please, try again");
                return View(loginDto);
            }

            var existingUserClaims = await _userManager.GetClaimsAsync(existingUser);
            if(!existingUserClaims.Any(c => c.Type == CustomClaims.FullName))
                await _userManager.AddClaimAsync(existingUser, new Claim(CustomClaims.FullName, existingUser.FullName));

            var result = await _signInManager.PasswordSignInAsync(existingUser.UserName, loginDto.Password, false, false);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Invalid login attempt");
            return View(loginDto);
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return View(registerDto);

            var newUser = new UserModel
            {
                FullName = $"{registerDto.FirstName} {registerDto.LastName}",
                Email = registerDto.Email,
                UserName = registerDto.Email,
                NormalizedEmail = registerDto.Email.ToUpperInvariant(),
                NormalizedUserName = registerDto.Email.ToUpperInvariant(),
                EmailConfirmed = false // Set to false unless you have email confirmation logic
            };

            // Check for existing user
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View(registerDto);
            }

            // Create the user
            var result = await _userManager.CreateAsync(newUser, registerDto.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(registerDto);
            }

            // Retrieve the saved user to ensure it has a valid Id
            var savedUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (savedUser == null || savedUser.Id <= 0)
            {
                ModelState.AddModelError(string.Empty, "Failed to retrieve the created user. Please try again.");
                return View(registerDto);
            }

            // Assign the role
            var roleResult = await _userManager.AddToRoleAsync(savedUser, AppRoles.User);
            if (!roleResult.Succeeded)
            {
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(registerDto);
            }

            // Sign in the user
            await _userManager.AddClaimAsync(savedUser, new Claim("Fullname", savedUser.FullName));
            await _signInManager.SignInAsync(savedUser, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordDto updateDto)
        {

            if (updateDto.NewPassword != updateDto.ConfirmPassword)
            {
                //passwords dont match
                TempData["PasswordError"] = "Passwords don't match";
                TempData["ActiveTab"] = "Password";
                return RedirectToAction("Index", "Settings");
            }

            var loggedInUser = await _userManager.GetUserAsync(User);
            //User is the current user from the HttpContext, which is set by the authentication middleware
            
            var isCurrentPassValid = await _userManager.CheckPasswordAsync(loggedInUser, updateDto.CurrentPassword);
            if (!isCurrentPassValid)
            {
                TempData["PasswordError"] = "Current password is invalid";
                TempData["ActiveTab"] = "Password";
                return RedirectToAction("Index", "Settings");
            }

            var result = await _userManager.ChangePasswordAsync(loggedInUser, updateDto.CurrentPassword, updateDto.NewPassword);
            if (result.Succeeded)
            {
                TempData["PasswordSuccess"] = "Password changed successfully";
                TempData["ActiveTab"] = "Password";
                await _signInManager.RefreshSignInAsync(loggedInUser);
                return RedirectToAction("Index", "Settings");
            }
            return RedirectToAction("Index", "Settings");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto updateDto)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (loggedInUser == null)
            {
                return RedirectToAction("Login");
            }

            loggedInUser.FullName = updateDto.Fullname;
            loggedInUser.UserName = updateDto.Username;
            loggedInUser.Bio = updateDto.Bio;

            var result = await _userManager.UpdateAsync(loggedInUser);
            if (!result.Succeeded)
            {
                TempData["UserProfileError"] = "Failed to update profile. Please try again.";
                TempData["ActiveTab"] = "Profile";
            }

            await _signInManager.RefreshSignInAsync(loggedInUser);
            return RedirectToAction("Index", "Settings");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}