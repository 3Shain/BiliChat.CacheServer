using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SanShain.Bilichat.Services;
using SanShain.Bilichat.Models;

namespace SanShain.Bilichat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatController : Controller
    {
        public CacheService Cache { get; set; }

        public BilichatContext Db { get; set; }

        public StatController(CacheService cache, BilichatContext db)
        {
            Cache = cache;
            Db = db;
        }

        [HttpGet("{roomid}")]
        public async Task<IActionResult> Get(int roomid)
        {
            if (roomid == 0)
            {
                return BadRequest();
            }
            var ret = await Cache.GetLiveInfo(roomid);
            if (ret != null)
            {
                var usr = await Cache.GetUserInfo(ret.uid);
                await Db.Entrys.AddAsync(new Entry()
                {
                    user_id = usr.mid,
                    user_name = usr.name,
                    user_intro = usr.sign,
                    entry_time = DateTime.Now,
                    room_id = ret.room_id
                });
                await Db.SaveChangesAsync();
                return Json(new
                {
                    ret.uid,
                    ret.room_id,
                    ret.live_status,
                    ret.live_time,
                    ret.is_sp,
                    ret.special_type,
                    config = new
                    {
                    }
                });
            }
            else
            {
                throw new Exception();
            }
        }

        public static long[] blackList = new long[]
        {
            405793756,405793791,405793975,405794141,405794153,405794222,405794250,405794260,405794261,405794412,405794418,405794616,405794733,405794777,405794907,405794966,405795017,405795040,405795042,405795135,405795208,405795242,405795300,405795387,405795451,405795453,405795525,405795618,405795745,405795756,405795855,405795857,405795980,405796030,405796125,405796127,405796187,405796294,405796369,405796586,405796669,405796902,405796925,405793739,405793884,405793958,405793996,405794110,405794169,405794214,405794230,405794284,405794370,405794569,405794709,405794835,405794889,405795067,405795199,405795196,405795246,405795421,405795443,405795837,405795907,405796097,405796288,405796439,405796486,405796735,405796956,405793981,405794612,405794663,405795172,405795889,405796072,405796400,405796930,405796942,405793759,405794220,405794256,405794732,405795037,405795087,405795282,405795400
        };
    }
}
