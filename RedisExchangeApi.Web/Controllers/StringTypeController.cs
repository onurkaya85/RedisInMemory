using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;

        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = redisService.GetDb(0);
        }

        public IActionResult Index()
        {
            db.StringSet("name", "Onur Kaya");
            db.StringSet("Visitors", 10);

            return View();
        }

        public IActionResult Show()
        {
            var value = db.StringGet("name");

            if(value.HasValue)
            {
                ViewBag.value = value.ToString();
            }

            // 5'er arttırır
            var cauntInc = db.StringIncrement("Visitors", 5);

            // Async
            // 1'er azaltır
            var countDec = db.StringDecrementAsync("Visitors", 1).Result;

            //İlk Üç Karakter
            var rangeValue = db.StringGetRange("name", 0, 3);
            
            //Length
            var length = db.StringLength("name");


            return View();
        }
    }
}
