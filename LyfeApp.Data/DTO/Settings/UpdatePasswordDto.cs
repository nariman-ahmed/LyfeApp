using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyfeApp.Data.DTO.Settings
{
    public class UpdatePasswordDto
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

    }
}