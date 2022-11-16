using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;

namespace RedisExchangeApi.Web.Controllers
{
    public class ListTypeController : Controller
    {

        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private string listKey = "names";

        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = redisService.GetDb(1);
        }
        public IActionResult Index()
        {
            var nameList = new List<string>();

            if(db.KeyExists(listKey))
            {
                db.ListRange(listKey).ToList()
                    .ForEach(x =>
                    {
                        nameList.Add(x.ToString());
                    });
            }

            return View(nameList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            //Adds to end of the list
            db.ListRightPush(listKey, name);

            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string name)
        {
            //Let's use Async 
            db.ListRemoveAsync(listKey, name).Wait();

            return RedirectToAction("Index");
        }

        public IActionResult DeleteFirstItem()
        {
            db.ListLeftPop(listKey);
            return RedirectToAction("Index");
        }
    }
}
