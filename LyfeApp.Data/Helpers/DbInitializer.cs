using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LyfeApp.Data.Models;

namespace LyfeApp.Data.Helpers
{
    public class DbInitializer
    {
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