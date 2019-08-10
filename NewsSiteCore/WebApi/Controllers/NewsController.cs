using Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/News")]
    public class NewsController : Controller
    {
        private readonly INewsContextProvider _dbProvider;

        public NewsController(INewsContextProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        [Route("test")]
        public IActionResult test()
        {
            return Json(true);
        }

        [Authorize]
        [Route("test2")]
        public IActionResult test2()
        {
            return Json(true);
        }

        [Authorize(Roles = "Admin")]
        [Route("test3")]
        public async Task<IActionResult> test3()
        {
            var k = await _dbProvider.Posts.GetListFromCacheAsync();
            return Json(true);
        }
    }
}
