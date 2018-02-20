using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using prova_cache_redis.Models;

using Microsoft.Extensions.Caching.Distributed; // package Microsoft.Extensions.Caching.Redis
using System.Text;

namespace prova_cache_redis.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        private readonly IDistributedCache _distributedCache;

        public HomeController(ILogger<HomeController> logger, IDistributedCache distributedCache)
        {            
            _logger = logger;
            _distributedCache = distributedCache;
        }

        public IActionResult Index()
        {


            _logger.LogWarning("richiesta pagina HomeIndex(information) alle " + DateTime.Now);
            _logger.LogInformation("richiesta pagina HomeIndex (information)");

            string chiave = "orario";
            string valore = DateTime.Now.ToLongTimeString();

            byte[] valore_bin = Encoding.UTF8.GetBytes(valore);
            //metto il valore in cache
            _distributedCache.Set(chiave, valore_bin);

            //lorecupero dalla cache
            string valore_dalla_cache = Encoding.UTF8.GetString(_distributedCache.Get(chiave));
            _logger.LogWarning("Recuperato da Redis orario="+ valore_dalla_cache);

            return View();
        }

        public IActionResult About()
        {
            _logger.LogInformation("richiesta pagina HomeAbout (information)");
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
