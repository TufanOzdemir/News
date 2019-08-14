using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebPanel.Models;

namespace WebPanel.Controllers
{
    public class HomeController : Controller
    {
        private readonly INewsContextProvider _newsContextProvider;

        public HomeController(INewsContextProvider newsContextProvider)
        {
            _newsContextProvider = newsContextProvider;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var cities = await _newsContextProvider.Cities.GetListAsync();
            var news = await _newsContextProvider.Posts.GetListAsync();
            var result = new DashboardViewModel
            {
                CityCount = cities.IsSuccess ? cities.Data.Count : 0,
                DailyNewsCount = news.IsSuccess ? news.Data.Where(c => c.CreateDate > DateTime.Now.Date).Count() : 0,
                TotalNewsCount = news.IsSuccess ? news.Data.Count : 0
            };

            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
