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
    public class RedisController : Controller
    {
        private readonly ILogger _logger;
        //private readonly IDistributedCache _distributedCache;
        public readonly IConnectionMultiplexer _mio_redis_connessione;

        //public HomeController(ILogger<HomeController> logger, IDistributedCache distributedCache)
        public RedisController(ILogger<RedisController> logger, IConnectionMultiplexer mio_redis_connessione)
        {            
            _logger = logger;
            //  _distributedCache = distributedCache;
            _mio_redis_connessione = mio_redis_connessione;
        }

        public IActionResult Index()
        {


            _logger.LogWarning("richiesta pagina Redis Index alle " + DateTime.Now);
            

            return View();
        }


        public IActionResult Redis()
        {
            _logger.LogWarning("richiesta pagina Redis (information)");           

            try
            {

                //http://taswar.zeytinsoft.com/redis-for-net-developer-connecting-with-c/

                

                string chiave = "orario";
                string valore = DateTime.Now.ToLongTimeString();

                //tutti i possibili paramteri : https://stackexchange.github.io/StackExchange.Redis/Configuration.html
                
                IDatabase connessione_db = _mio_redis_connessione.GetDatabase(); //connessione col db

                _logger.LogWarning("inserisco nella  chiave=" + chiave + " il valore=" + valore);
                if (connessione_db.StringSet(chiave, valore)) //scrive nuovo valore in chace, anche se esiste gia'
                {
                    string val = connessione_db.StringGet(chiave); //recuepra da
                    _logger.LogWarning("recuperato per la chiave="+ chiave + " valore=" +val);
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
