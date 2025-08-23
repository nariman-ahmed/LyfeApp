using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LyfeApp.Data.Services;

namespace LyfeApp.Controllers
{
    public class FriendsController
    {
        private readonly IFriendsService _friendsService;
        public FriendsController(IFriendsService friendsService)
        {
            _friendsService = friendsService;
        }
    }
}