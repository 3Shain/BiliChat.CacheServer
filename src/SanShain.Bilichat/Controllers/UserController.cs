using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SanShain.Bilichat.Services;

namespace SanShain.Bilichat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        public CacheService Cache { get; set; }

        [HttpGet("{mid}")]
        public async Task<IActionResult> Get(int mid)
        {
            if (mid == 0)
            {
                return BadRequest();
            }
            var ret = await Cache.GetUserInfo(mid);
            if (ret != null)
            {
                return Json(ret);
            }
            else
            {
                throw new Exception();
            }
        }

        public UserController(CacheService cache)
        {
            Cache = cache;
        }
    }
}