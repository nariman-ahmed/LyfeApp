using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LyfeApp.Data.Models;

namespace LyfeApp.Data.DTO.Users
{
    public class CountOfUserFriendsDto
    {
        //da dto hayetla3 lel view, msh hayeb2a extracted meno
        //ayza 3adad el as7ab el user. w da hayeb2a return tyoe el duggested users function
        public UserModel User { get; set; }
        public int FriendsCount { get; set; }
    }
}