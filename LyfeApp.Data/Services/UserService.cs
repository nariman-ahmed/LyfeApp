using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LyfeApp.Data.Models;

namespace LyfeApp.Data.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<UserModel> GetUserAsync(int loggedInUserId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == loggedInUserId) ?? new UserModel();
        }

        public async Task UpdateUserProfilePic(int loggedInUserId, string newImageUrl)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == loggedInUserId);

            if (user != null)
            {
                user.ProfilePicUrl = newImageUrl;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
        }

        // public async Task UpdateProfile(int loggedInUserId, string fullname, string username, string email, string bio)
        // {
        //     var user = _context.Users.FirstOrDefaultAsync(u => u.Id == loggedInUserId);

        //     if (user != null)
        //     {
        //         if (fullname != null)
        //         {
        //             user.Fullname = fullname;
        //         }
        //         if (username != null)
        //         {
        //             user.Username = username;
        //         }
        //         if (email != null)
        //         {
        //             user.Email = email;
        //         }
        //         if (bio != null)
        //         {
        //             user.Bio = bio;
        //         }

        //         _context.Users.Update(user);
        //         await _context.SaveChangesAsync();
        //     }
        // }

        
    }
}