using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project1.WebUI.Models;

namespace Project1.WebUI.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Default Controller and initial entry point of the program.
        /// The Index resets the error cookies and displays the front page.
        /// </summary>
        /// <returns>Index View</returns>
        public IActionResult Index()
        {
            HttpContext.Response.Cookies.Append("error", "");
            return View();
        }
        /// <summary>
        /// Default MVC Privacy Page.
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            return View();
        }
        /// <summary>
        /// Default MVC Error Action Method.
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
