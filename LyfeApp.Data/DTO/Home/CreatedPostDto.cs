using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LyfeApp.Data.DTO.Home
{
    public class CreatedPostDto
    {
        public string Content { get; set; }

        public IFormFile Image { get; set; }
    }
}