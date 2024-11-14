using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisCache.Cache;
using RedisCache.Model;
using RedisCache.Data;

namespace RedisCache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DbContextClass _dbContext;
        private readonly ICacheService _cacheService;
        private static object _lock = new object();

        public ProductController(DbContextClass dbContext, ICacheService cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }
       
        [HttpGet("product")]
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
        [HttpPost("addproduct")]
        public async Task<Product> Post(Product value)
        {
            var obj = await _dbContext.Products.AddAsync(value);
            _cacheService.RemoveData("product");
            _dbContext.SaveChanges();
            return obj.Entity;
        }
        [HttpPut("updateproduct")]
        public void Put(Product product)
        {
            _dbContext.Products.Update(product);
            _cacheService.RemoveData("product");
            _dbContext.SaveChanges();
        }
        [HttpDelete("deleteproduct")]
        public void Delete(int Id)
        {
            var filteredData = _dbContext.Products.Where(x => x.ProductId == Id).FirstOrDefault();
            _dbContext.Remove(filteredData);
            _cacheService.RemoveData("product");
            _dbContext.SaveChanges();
        }


        //// GET: ProductController
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: ProductController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: ProductController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: ProductController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: ProductController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: ProductController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: ProductController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: ProductController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
