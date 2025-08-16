using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace LyfeApp.Data.DTO.Settings
{
    public class UpdateProfilePicDto
    {
        public IFormFile NewProfilePic { get; set; }
    }
}