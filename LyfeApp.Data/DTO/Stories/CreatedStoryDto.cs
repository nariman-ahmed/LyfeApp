using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LyfeApp.Data.DTO.Stories
{
    public class CreatedStoryDto
    {
        public IFormFile Image { get; set; }

    }
}