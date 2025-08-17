using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyfeApp.Data.Helpers.Constants
{
    public static class AppRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";

        //things that can accessed by both
        public static readonly IReadOnlyList<string> All = new[] { Admin, User };

    }
}