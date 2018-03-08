using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using prova_cache_redis.Models;

//using Microsoft.Extensions.Caching.Distributed; // package Microsoft.Extensions.Caching.Redis
using StackExchange.Redis; //c'è gia' il package StackExchange.Redis in Micorsoft.AspNEtcore.All, SE NO StackExchange.Redis

using System.Text;

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


            _logger.LogWarning("richiesta pagina HomeIndex(information) alle " + DateTime.Now);
            _logger.LogInformation("richiesta pagina HomeIndex (information)");

            string chiave = "orario";
            string valore = DateTime.Now.ToLongTimeString();

            byte[] valore_bin = Encoding.UTF8.GetBytes(valore);
            //metto il valore in cache
            //_distributedCache.Set(chiave, valore_bin);

            //lo recupero dalla cache
            //string valore_dalla_cache = Encoding.UTF8.GetString(_distributedCache.Get(chiave));
            //_logger.LogWarning("Recuperato da Redis orario="+ valore_dalla_cache);

            return View();
        }



        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
