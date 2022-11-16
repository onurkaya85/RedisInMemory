using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisnET7App.Cache;
using RedisNet7App.Api.Model;
using RedisNet7App.Api.Repository;
using RedisNet7App.Api.Services;

namespace RedisNet7App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _productService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            return Created(string.Empty,await _productService.CreateAsync(product));
        }
    }
}
