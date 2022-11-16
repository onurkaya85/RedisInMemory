using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly RedisService _redisService;
        protected readonly IDatabase db;
       
        public BaseController(RedisService redisService)
        {
            _redisService = redisService;
            db = redisService.GetDb(4);
        }
    }
}
