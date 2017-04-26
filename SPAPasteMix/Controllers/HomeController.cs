using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SPAPasteMix.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            return PartialView();
        }

        public IActionResult CreatePost()
        {
            return PartialView();
        }

        public IActionResult ReadPost()
        {
            return PartialView();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
