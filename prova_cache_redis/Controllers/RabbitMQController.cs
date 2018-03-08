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
    public class RabbitMQController : Controller
    {
        private readonly ILogger _logger;
        //private readonly IDistributedCache _distributedCache;
        //public readonly IConnectionMultiplexer _mio_redis_connessione;

        //public HomeController(ILogger<HomeController> logger, IDistributedCache distributedCache)
        //public RabbitMQController(ILogger<RabbitMQController> logger, IConnectionMultiplexer mio_redis_connessione)
        public RabbitMQController(ILogger<RabbitMQController> logger)
        {            
            _logger = logger;
            //  _distributedCache = distributedCache;
            // _mio_redis_connessione = mio_redis_connessione;
        }

        public IActionResult Index()
        {


            _logger.LogWarning("richiesta pagina RabbitMQ\\Index  alle " + DateTime.Now);
            
            return View();
        }


        //gestisce la GET  del bottone RabbitMQLanciaProducer,ritorna json
        [HttpGet]
        public IActionResult LanciaProducer(int StockId)
        {
            try
            {
                _logger.LogInformation("[LanciaProducer] da bottone lanciato RabbitMQ LanciaProducer");
             
                if ( true )
                {
                    
                    return Ok(new { successo = "Ok"});
                }
                else //non trovata
                {
                    return Ok(new { errore = "errore" });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("[LanciaProducer] Eccezzione : " + ex.Message);
                return Ok(new { errore = "Errore : " + ex.Message });
            }

        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
