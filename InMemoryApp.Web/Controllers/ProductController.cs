using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            //Getting cached data from memory with null check.

            //first method
            if (String.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            {
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            }

            //second method
            if (!_memoryCache.TryGetValue("zaman",out string zamancache)) 
            {
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            }
            return View();
        }


        public IActionResult Show()
        {
            //third method
            _memoryCache.GetOrCreate<string>("zaman", entry =>
            {
                return DateTime.Now.ToString();
            });

            ViewBag.zaman = _memoryCache.Get<string>("zaman");
            return View();
        }
    }
}
