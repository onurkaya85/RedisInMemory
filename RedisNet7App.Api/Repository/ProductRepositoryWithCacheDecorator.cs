using RedisnET7App.Cache;
using RedisNet7App.Api.Model;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisNet7App.Api.Repository
{
    public class ProductRepositoryWithCacheDecorator : IProductRepository
    {
        private const string productKey = "productCaches"; 
        private readonly IProductRepository _repository;
        private readonly RedisService _redisService;
        private readonly IDatabase _cacheRepository;

        public ProductRepositoryWithCacheDecorator(IProductRepository repository,RedisService redisService)
        {
            _repository = repository;
            _redisService = redisService;
            _cacheRepository = _redisService.GetDb(2);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            var newProduct = await _repository.CreateAsync(product);

            if(await _cacheRepository.KeyExistsAsync(productKey))
               await _cacheRepository.HashSetAsync(productKey, product.Id, JsonSerializer.Serialize(newProduct));

            return newProduct;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            if (!await _cacheRepository.KeyExistsAsync(productKey))
                return await LoadCacheFromDbAsync();

            var products = new List<Product>();
            var cacheProducts = await _cacheRepository.HashGetAllAsync(productKey);
            foreach (var item in cacheProducts.ToList())
            {
                var product = JsonSerializer.Deserialize<Product>(item.Value);
                products.Add(product);
            }

            return products;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            if(_cacheRepository.KeyExists(productKey))
            {
                var product = await _cacheRepository.HashGetAsync(productKey, id);
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
            }

            var products = await LoadCacheFromDbAsync();
            return products.FirstOrDefault(v => v.Id == id);
        }

        private async Task<List<Product>> LoadCacheFromDbAsync()
        {
            var products = await _repository.GetAllAsync();
            products.ForEach(v =>
            {
                _cacheRepository.HashSetAsync(productKey, v.Id, JsonSerializer.Serialize(v));
            });

            return products;
        }
    }
}
