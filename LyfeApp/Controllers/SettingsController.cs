using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LyfeApp.Controllers
{
    public class SettingsController : Controller
    {

        public SettingsController(ILogger<SettingsController> logger)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}