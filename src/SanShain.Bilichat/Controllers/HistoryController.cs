using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanShain.Bilichat.Models;

namespace SanShain.Bilichat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : Controller
    {
        public BilichatContext Db { get; set; }

        public HistoryController(BilichatContext db)
        {
            Db = db;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Json(await Db.Entrys.OrderByDescending(x => x.id).Take(30).Select(u =>
              new { u.user_id, u.user_intro, u.user_name, u.room_id, u.id, time = (u.entry_time.Ticks - (new DateTime(1970, 1, 1, 0, 0, 0)).Ticks) / 10000 }).ToArrayAsync());//时区代码很有问题,以后改
        }
    }
}