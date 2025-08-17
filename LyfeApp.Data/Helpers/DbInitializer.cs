using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LyfeApp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LyfeApp.Data.Helpers.Constants;


namespace LyfeApp.Data.Helpers
{
    public class DbInitializer
    {
        public static async Task SeedUsersAndRolesAsync(UserManager<UserModel> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            // Seed Roles
            if (!roleManager.Roles.Any())
            {
                foreach (var roleName in AppRoles.All)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole<int>(roleName));
                    }
                }
            }

            // Seed Users
            if (!userManager.Users.Any(u => !string.IsNullOrEmpty(u.Email)))
            {
                var userPassword = "Coding@1234?";
                var newUser = new UserModel
                {
                    UserName = "nariman.ahmed",
                    Email = "nari@gmail.com",
                    NormalizedEmail = "NARI@GMAIL.COM",
                    NormalizedUserName = "NARIMAN.AHMED",
                    FullName = "Nariman Ahmed",
                    ProfilePicUrl = "https://shorturl.at/EbnVL",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newUser, userPassword);
                if (result.Succeeded)
                {
                    var savedUser = await userManager.FindByEmailAsync(newUser.Email);
                    if (savedUser != null)
                    {
                        await userManager.AddToRoleAsync(savedUser, AppRoles.User);
                    }
                }

                var newAdmin = new UserModel
                {
                    UserName = "admin.admin",
                    Email = "admin@mariam.com",
                    NormalizedEmail = "ADMIN@MARIAM.COM",
                    NormalizedUserName = "ADMIN.ADMIN",
                    FullName = "Mariam Admin",
                    ProfilePicUrl = "https://shorturl.at/ad4em",
                    EmailConfirmed = true
                };

                var resultNewAdmin = await userManager.CreateAsync(newAdmin, userPassword);
                if (resultNewAdmin.Succeeded)
                {
                    var savedAdmin = await userManager.FindByEmailAsync(newAdmin.Email);
                    if (savedAdmin != null)
                    {
                        await userManager.AddToRoleAsync(savedAdmin, AppRoles.Admin);
                    }
                }
            }
        }

        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.Users.Any() && !context.Posts.Any())
            {
                // Seed Users
                var newUser = new UserModel
                {
                    FullName = "Mariam Gamal",
                    ProfilePicUrl = "https://shorturl.at/0NNmK"
                };

                await context.Users.AddAsync(newUser);
                await context.SaveChangesAsync();

                // Seed Posts
                var PostWithoutImage = new PostModel
                {
                    Content = "Heyyyy! This is a post without an image. This is actually loaded from the database.",
                    NumReports = 0,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    UserId = newUser.Id
                };

                var PostWithImage = new PostModel
                {
                    Content = "Yo yo yo! This time it's is a post with an image. This too is loaded from the database.",
                    ImageUrl = "https://shorturl.at/0CAoW",
                    NumReports = 0,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    UserId = newUser.Id
                };

                await context.Posts.AddRangeAsync(PostWithoutImage, PostWithImage);
                await context.SaveChangesAsync();

            }
        }
    }
}