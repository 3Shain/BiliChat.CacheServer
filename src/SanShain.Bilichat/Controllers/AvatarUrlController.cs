using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SanShain.Bilichat.Services;

namespace SanShain.Bilichat.Controllers
{
    [Route("api/avturl")]
    [ApiController]
    public class AvatarUrlController : Controller
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
                Response.Headers["Cache-Control"] = "public,max-age=" + 60 * 60 * 24 * 3;
                return Json(new
                {
                    ret.face
                });
            }
            else
            {
                throw new Exception();
            }
        }

        public AvatarUrlController(CacheService cache)
        {
            Cache = cache;
        }
    }
}