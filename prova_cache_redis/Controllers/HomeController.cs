using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using prova_cache_redis.Models;

namespace prova_cache_redis.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;

       public HomeController(ILogger<HomeController> logger)
        {            
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogWarning("richiesta pagina HomeIndex(information)");
            _logger.LogInformation("richiesta pagina HomeIndex (information)");

            return View();
        }

        public IActionResult About()
        {
            _logger.LogInformation("richiesta pagina Home\About (information)");
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
