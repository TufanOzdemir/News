using Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/[Controller]")]
    public class CityController : Controller
    {
        private readonly INewsContextProvider _dbProvider;

        public CityController(INewsContextProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            return Json(await _dbProvider.Cities.GetListFromCacheAsync());
        }

        [Route("Get/{id}")]
        public async Task<IActionResult> Get(params int[] id)
        {
            var result = await _dbProvider.Cities.GetListFromCacheAsync();
            result.Data = result.Data.Where(c => id.Contains(c.Id)).ToList();
            return Json(result);
        }

        [Route("Save")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Save([FromBody]City city)
        {
            var result = await _dbProvider.Cities.SaveOrUpdateClearCacheAsync(city);
            return Json(result);
        }

        [Route("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _dbProvider.Cities.DeleteAsync(c => c.Id == id);
            return Json(result);
        }
    }
}
