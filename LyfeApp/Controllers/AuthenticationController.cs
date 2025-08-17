using LyfeApp.Data.Helpers.Constants;
using LyfeApp.Data.Models;
using LyfeApp.Data.DTO.Authentication;
using LyfeApp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
            if (!ModelState.IsValid)
            {
                return View(loginDto);  //hayraga3 el dto lel view 3shan yebayen el errors
            }

            var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false);
            //first false is for the isPersistent
            //2nd one is for if u want to log out the user after incorrect loding details.

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

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
            await _signInManager.SignInAsync(savedUser, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}