using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SanShain.Bilichat.Services;

namespace SanShain.Bilichat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvatarController : Controller
    {
        public CacheService Cache { get; set; }

        [HttpGet("{mid}")]
        public async Task<IActionResult> Get(int mid)
        {
            if (mid == 0)
            {
                return NotFound();
            }
            byte[] rawData = await Cache.GetJpegAvatar(mid);
            if (rawData == null)
            {
                return NotFound();
            }
            else if (rawData.Length == 0)
            {
                return Redirect("http://static.hdslb.com/images/member/noface.gif");
            }
            Response.Headers["Cache-Control"] = "public,max-age=" + 60 * 60 * 24 * 3;
            return File(rawData, "image/jpeg");
        }

        public AvatarController(CacheService cache)
        {
            Cache = cache;
        }
    }
}