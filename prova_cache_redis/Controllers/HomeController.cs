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
        //private readonly IDistributedCache _distributedCache;

        //public HomeController(ILogger<HomeController> logger, IDistributedCache distributedCache)
        public HomeController(ILogger<HomeController> logger)
        {            
            _logger = logger;
          //  _distributedCache = distributedCache;
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


        public IActionResult Redis()
        {
            _logger.LogInformation("richiesta pagina Redis (information)");


            try
            {

                //http://taswar.zeytinsoft.com/redis-for-net-developer-connecting-with-c/

                string chiave = "orario";
                string valore = DateTime.Now.ToLongTimeString();

                string connection_string = "redis";
                _logger.LogInformation("provo a connettermi con : " + connection_string);
                ConnectionMultiplexer mio_client_redis = ConnectionMultiplexer.Connect("localhost");

                IDatabase connessione_db = mio_client_redis.GetDatabase(); //connessione col db

                _logger.LogInformation("inserisco nella  chiave=" + chiave + " il valore=" + valore);
                if (connessione_db.StringSet(chiave, valore)) //scrive nuovo valore in chace, anche se esiste gia'
                {
                    string val = connessione_db.StringGet(chiave); //recuepra da
                    _logger.LogInformation("recuperato per la chiave="+ chiave + " valore=" +val);
                }

            }
            catch (Exception ex)
            {
                _logger.LogWarning("Eccezzione nel uso Redis client :" + ex.Message);
            }

            ViewData["Message"] = "Your contact page.";

            return View();
        }

        
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
