namespace RedisCache.Model
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }

        public string ProductCategory { get; set; }
        public int Quantity { get; set; }
    }
}
