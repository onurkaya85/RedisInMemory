using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeApi.Web.Controllers
{
    //Key Value değerlerini dictionary Tipinde tutar
    public class HashTypeController : BaseController
    {

        private string hashKey { get; set; } = "sozluk";

        public HashTypeController(RedisService redisService) : base(redisService)
        {

        }

        public IActionResult Index()
        {
            var dic = new Dictionary<string, string>();
            if(db.KeyExists(hashKey))
            {
                db.HashGetAll(hashKey).ToList().ForEach(v =>
                {
                    dic.Add(v.Name, v.Value);
                });
            }
            return View(dic);
        }

        [HttpPost]
        public IActionResult Add(string name,string value)
        {
            db.HashSet(hashKey, name,value);
            return RedirectToAction("Index");
        }

        //Let's use Async 
        public async Task<IActionResult> DeleteItem(string name)
        {
            await db.HashDeleteAsync(hashKey, name);

            return RedirectToAction("Index");
        }
    }
}
