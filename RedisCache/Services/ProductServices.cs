using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using RedisCache.Cache;
using RedisCache.Data;
using RedisCache.Model;

namespace RedisCache.Services
{
    public class ProductServices : IProduct
    {
        private readonly DbContextClass _dbContext;
        private readonly ICacheService _cacheService;
        private static object _lock = new object();
        public ProductServices(ICacheService cacheService, DbContextClass dbContext) {
        
            _dbContext = dbContext;
            _cacheService = cacheService;
        } 

        public Product GetDataById(int id) {

            Product filteredData =new Product() ;
            var cacheData = _cacheService.GetData<IEnumerable<Product>>("product").Where(x => x.ProductId == id);
            if (cacheData != null)
            {
                filteredData = cacheData.FirstOrDefault(x => x.ProductId == id);
                return filteredData;
            }
            filteredData = _dbContext.Products.Where(x => x.ProductId == id).FirstOrDefault();
            return filteredData;
        }

        public List<Product> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> Get()
        {
            var cacheData = _cacheService.GetData<IEnumerable<Product>>("product");
            if (cacheData != null)
            {
                return cacheData;
            }
            lock (_lock)
            {
                var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
                cacheData = _dbContext.Products.ToList();
                _cacheService.SetData<IEnumerable<Product>>("product", cacheData, expirationTime);
            }
            return cacheData;
        }
    }
}
