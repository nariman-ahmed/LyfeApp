using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyfeApp.Data.DTO.Home
{
    public class AddNewCommentDto
    {
        public int PostId { get; set; }
        public string Content { get; set; }
    }
}