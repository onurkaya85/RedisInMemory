using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeApi.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private string listKey = "hashnames";

        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = redisService.GetDb(2);
        }

        public IActionResult Index()
        {
            HashSet<string> nameList = new HashSet<string>();
            if(db.KeyExists(listKey))
            {
                db.SetMembers(listKey).ToList().ForEach(v =>
                {
                    nameList.Add(v.ToString());
                });
            }
            return View(nameList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            //AbsoluteExpiration
            if(!db.KeyExists(listKey))
            {
                db.KeyExpire(listKey, DateTime.Now.AddMinutes(5));
            }

            //SlidingExpression. TimeOut always restarts
            //db.KeyExpire(listKey, DateTime.Now.AddMinutes(5));

            db.SetAdd(listKey, name);

            return RedirectToAction("Index");
        }

        //Let's use Async 
        public async Task<IActionResult> DeleteItem(string name)
        {
            await db.SetRemoveAsync(listKey, name);

            return RedirectToAction("Index");
        }
    }
}
