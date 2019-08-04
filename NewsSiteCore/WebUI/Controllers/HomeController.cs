using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DbModels;
using Repository.News.Interfaces;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        INewsContextProvider _newsContextProvider;
        public HomeController(INewsContextProvider newsContextProvider)
        {
            _newsContextProvider = newsContextProvider;
        }
        public IActionResult Index()
        {
            var k = _newsContextProvider.Context.Notifications.ToList();
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
