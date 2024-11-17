using RedisCache.Model;

namespace RedisCache.Services
{
    public interface IProduct
    {
        public Product GetDataById(int id);
        public List<Product>GetAll();

        public IEnumerable<Product> Get();





}