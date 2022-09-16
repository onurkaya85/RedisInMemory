using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            //1. yol
            //if(string.IsNullOrEmpty(_memoryCache.Get<string>("time")))
            //    _memoryCache.Set<string>("time", DateTime.Now.ToString());

            //2.Yol
            _memoryCache.Remove("time");
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
            {

                //AbsoluteExpiration = DateTime.Now.AddMinutes(1),
                //SlidingExpiration = TimeSpan.FromSeconds(10),
                AbsoluteExpiration = DateTime.Now.AddSeconds(10),
                Priority = CacheItemPriority.High
            };

            //Cache in neden silindiğini söyler
            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set<string>("callback", $"{key}:{value}=> sebep: {reason}");
            });

            _memoryCache.Set<string>("time", DateTime.Now.ToString(), options);


            var product = new Product
            {
                Id = 1,
                Name = "Kalem",
                Price = 200
            };

            _memoryCache.Set<Product>("product:1", product);

            return View();
        }

        public IActionResult Time()
        {
            //_memoryCache.Remove("time");
            //_memoryCache.GetOrCreate<string>("time", entry =>
            //{
            //    entry.Priority = CacheItemPriority.High;
            //    return DateTime.Now.ToString();
            //});

            //var cache = _memoryCache.Get<string>("time");
            _memoryCache.TryGetValue("time", out string value);
            ViewBag.time = value;

            _memoryCache.TryGetValue("callback", out string callback);
            ViewBag.callback = callback;

            _memoryCache.TryGetValue("product:1", out Product product);
            ViewBag.product = product;

            return View();
        }
    }
}
