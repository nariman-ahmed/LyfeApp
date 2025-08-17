using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyfeApp.Data.DTO.Settings
{
    public class UpdateProfileDto
    {
        //EVERYTHING COMING FROM THE VIEW MUST HAVE THE SAME NAME AS DECLARED IN THE CSHTML FILE 
        //ex->  <...name="EmailAddress"...> BAS MSH CASE SENSITIVE
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Bio { get; set; }
    }
}