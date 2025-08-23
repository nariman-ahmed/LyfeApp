using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LyfeApp.Data.Models;

namespace LyfeApp.Data.DTO.Users
{
    public class UserProfileDetailsDto
    {
        public UserModel User { get; set; }
        public List<PostModel> Posts { get; set; }
    }
}