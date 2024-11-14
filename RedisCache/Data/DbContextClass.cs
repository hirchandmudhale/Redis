namespace RedisCache.Data
{
    using Microsoft.EntityFrameworkCore;
    using RedisCache.Model;
    public class DbContextClass : DbContext
    {
        public DbContextClass(DbContextOptions<DbContextClass> options) : base(options) { }
        public DbSet<Product> Products
        {
            get;
            set;
        }
    }
}
