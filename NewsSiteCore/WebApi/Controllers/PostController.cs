using Extensions;
using Interfaces.ResultModel;
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
    [Produces("application/json")]
    [Route("api/[Controller]")]
    public class PostController : Controller
    {
        private readonly INewsContextProvider _dbProvider;

        public PostController(INewsContextProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        [Route("Get")]
        public async Task<IActionResult> Get(int[] cityIds)
        {
            Result<List<Post>> result = await _dbProvider.Posts.GetListFromCacheAsync();
            if (cityIds.Any())
            {
                result.Data = result.Data.Where(c=> cityIds.Contains(c.CityId)).ToList();
            }
            return Json(result);
        }

        [Route("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _dbProvider.Posts.GetListFromCacheAsync();
            result.Data = result.Data.Where(c => c.Id == id).ToList();
            return Json(result);
        }

        [Route("Save")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Save([FromBody]Post post)
        {
            post.UserId = User.GetUserId();
            var result = await _dbProvider.Posts.SaveOrUpdateClearCacheAsync(post);
            return Json(result);
        }

        [Route("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _dbProvider.Posts.DeleteAsync(c => c.Id == id);
            return Json(result);
        }
    }
}
