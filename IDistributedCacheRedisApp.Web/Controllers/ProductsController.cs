using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }


        #region Sync

        public IActionResult Index()
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(1)
            };

            _distributedCache.SetString("name", "onur kaya", cacheOptions);
            return View();
        }

        public IActionResult Show()
        {
            string name = _distributedCache.GetString("name");
            ViewBag.name = name;
            return View();
        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("name");
            return View();
        }

        #endregion

        #region Async

        public async Task<IActionResult> AsyncIndex()
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(1)
            };

            await _distributedCache.SetStringAsync("name", "onur kaya", cacheOptions);
            return View();
        }

        public async Task<IActionResult> ShowAsync()
        {
            string name = await _distributedCache.GetStringAsync("name");
            ViewBag.name = name;
            return View();
        }

        public async Task<IActionResult> RemoveAsync()
        {
            await _distributedCache.RemoveAsync("name");
            return View();
        }
        #endregion

        #region ComplexTypes

        public async Task<IActionResult> ComplexTypeIndex()
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(5)
            };

            var product = new Product
            {
                Id = 1,
                Name = "Pen",
                Price = 20
            };

            //1. Yöntem
            //string jsonProduct = JsonConvert.SerializeObject(product);
            //await _distributedCache.SetStringAsync("product:1", jsonProduct, cacheOptions);

            //2. Yöntem
            string jsonProduct = JsonConvert.SerializeObject(product);
            byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);

            await _distributedCache.SetAsync("product:1", byteProduct);

            return View();
        }

        public async Task<IActionResult> ComplexTypeShow()
        {

            //1. Yöntem
            //string jsonProduct = await _distributedCache.GetStringAsync("product:1");
            //var product = JsonConvert.DeserializeObject<Product>(jsonProduct);


            //2. Yöntem
            byte[] byteProduct = await _distributedCache.GetAsync("product:1");
            string jsonProduct = Encoding.UTF8.GetString(byteProduct);
            var product = JsonConvert.DeserializeObject<Product>(jsonProduct);

            ViewBag.product = product;

            return View();
        }

        #endregion

        #region ImagePdfCache

        public async Task<IActionResult> ImageCache()
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(5)
            };

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/car.jpg");
            byte[] imageByte = System.IO.File.ReadAllBytes(path);
            await _distributedCache.SetAsync("image", imageByte,cacheOptions);

            return View();
        }

        public async Task<IActionResult> ImageUrl()
        {
            byte[] imageByte = await _distributedCache.GetAsync("image");
            return File(imageByte, "image/jpg");
        }

        #endregion
    }
}
