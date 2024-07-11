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
            //if (String.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            //{
            //    _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            //}

            //second method

            //defining the time period of cached value which we will create.
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            // --> giving an absolute live time to cache
            //options.AbsoluteExpiration = DateTime.Now.AddSeconds(30);

            // --> givin and sliding live time to cache. This will extends like given time whether you trigger the action method.
            options.SlidingExpiration = TimeSpan.FromSeconds(10);
            // --> giving priority to cache (CacheItemPriority.NeverRemove making cached value permantent, becareful when you use this.)
            options.Priority = CacheItemPriority.High;


            //using RegisterPostEvicitionCallBack Method

            options.RegisterPostEvictionCallback((key,value,reason,state) => {
                _memoryCache.Set("callback", $"{key} --> {value}  ==> sebep: {reason}");
            });



            _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);

            return View();
        }


        public IActionResult Show()
        {
            ////third method
            //_memoryCache.GetOrCreate<string>("zaman", entry =>
            //{
            //    return DateTime.Now.ToString();
            //});

            _memoryCache.TryGetValue("zaman", out string zamancache);
            _memoryCache.TryGetValue("callback", out string callback);
            ViewBag.callback = callback;
            ViewBag.zaman = zamancache;


            return View();
        }
    }
}
