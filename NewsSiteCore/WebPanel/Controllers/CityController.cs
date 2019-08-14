using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interfaces.ResultModel;
using Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DbModels;
using WebPanel.Extensions;

namespace WebPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CityController : Controller
    {
        private readonly INewsContextProvider _newsContextProvider;

        public CityController(INewsContextProvider newsContextProvider)
        {
            _newsContextProvider = newsContextProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> List()
        {
            var result = await _newsContextProvider.Cities.GetListAsync();
            result.Html = await PartialView("_List", result).ToString(ControllerContext);
            return Json(result);
        }

        public async Task<IActionResult> Create()
        {
            var result = new Result<City>();
            result.Html = await PartialView("_Save", result).ToString(ControllerContext);
            return Json(result);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _newsContextProvider.Cities.FirstOrDefaultAsync(c => c.Id == id);
            result.Html = await PartialView("_Save", result).ToString(ControllerContext);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Result<City> model)
        {
            var result = await _newsContextProvider.Cities.SaveOrUpdateAsync(model.Data);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _newsContextProvider.Cities.DeleteAsync(c=>c.Id == id);
            return Json(result);
        }
    }
}