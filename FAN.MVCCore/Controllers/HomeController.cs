using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FAN.MVCCore.Models;

namespace FAN.MVCCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            string url = base.Url.Action();
            string url2 = base.Url.Action("Index",new { id=10});
            string url3 = base.Url.Action("About","Home");
            string url4 = base.Url.Action("About", "Home1");

            return View();
        }
        [Route("h/a")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
