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
                    config=GetConfig(ret.room_id)
                });
            }
            else
            {
                throw new Exception();
            }
        }

        public object GetConfig(int room_id)
        {
            if (room_id == 14917277)
            {
                return new
                {
                    customEmotions=new object[]
                    {
                        new
                        {
                            command="理解理解.jpg",
                            source="https://bilichat.3shain.com/images/rikai.jpg"
                        },new
                        {
                            command="点名夸奖.jpg",
                            source="https://bilichat.3shain.com/images/kua.jpg"
                        },new
                        {
                            command="余裕余裕.jpg",
                            source="https://bilichat.3shain.com/images/yoyu.jpg"
                        },new
                        {
                            command="这种事情很奇怪啊.jpg",
                            source="https://bilichat.3shain.com/images/qg.jpg"
                        },new
                        {
                            command="？？？？",
                            source="https://bilichat.3shain.com/images/n.gif"
                        },new
                        {
                            command="？？？？？",
                            source="https://bilichat.3shain.com/images/nn.gif"
                        },new
                        {
                            command="不也挺好吗.jpg",
                            source="https://bilichat.3shain.com/images/by.jpg"
                        },new
                        {
                            command="完全理解.jpg",
                            source="https://bilichat.3shain.com/images/gzrk.jpg"
                        }
                    }
                };
            }
            return new object();
        }
    }
}