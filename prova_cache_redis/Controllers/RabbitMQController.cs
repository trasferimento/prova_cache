using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using prova_cache_redis.Models;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace prova_cache_redis.Controllers
{
    public class RabbitMQController : Controller
    {
        private readonly ILogger _logger;
        private readonly IConnection _mioRabbitConnessione;
        //private readonly IDistributedCache _distributedCache;
        //public readonly IConnectionMultiplexer _mio_redis_connessione;

        //public HomeController(ILogger<HomeController> logger, IDistributedCache distributedCache)
        //public RabbitMQController(ILogger<RabbitMQController> logger, IConnectionMultiplexer mio_redis_connessione)
        public RabbitMQController(ILogger<RabbitMQController> logger, IConnection mioRabbitConnessione)
        {
            _logger = logger;
            _mioRabbitConnessione = mioRabbitConnessione;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("richiesta pagina RabbitMQ\\Index  alle " + DateTime.Now);

            return View();
        }


        //gestisce la GET  del bottone RabbitMQLanciaProducer,ritorna json
        [HttpGet]
        public IActionResult LanciaProducer(int StockId)
        {
            string nome_coda = "hello";
            string message = "Sono le " + DateTime.Now.ToLongTimeString();


            try
            {
                _logger.LogInformation("[LanciaProducer] LanciaProducer");
                
                using (var canale = _mioRabbitConnessione.CreateModel())
                {
                    //creo la queue se non esistesse gia'
                    _logger.LogInformation("[LanciaProducer] Creo la queue =" + nome_coda + " se non esistesse gia'");
                    canale.QueueDeclare(queue: nome_coda,
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var body = Encoding.UTF8.GetBytes(message);
                    _logger.LogInformation("[LanciaProducer] Invio il messaggio =" + message);
                    canale.BasicPublish(exchange: "",
                                         routingKey: nome_coda,
                                         basicProperties: null,
                                         body: body);
                
                }

                return Ok(new { successo = "Nella coda=" + nome_coda + " inserito message=" + message });
                

            }
            catch (Exception ex)
            {
                _logger.LogError("[LanciaProducer] Eccezzione nell'inserire nella coda=" + nome_coda + " message=" + message + " : " + ex.Message);
                return Ok(new { errore = "Errore : " + ex.Message });
            }

        }

        //gestisce la GET  del bottone RabbitMQLanciaProducer,ritorna json
        [HttpGet]
        public IActionResult LanciaConsumer(int StockId)
        {
            string nome_coda = "hello";
            string messaggio = "";
            try
            {
                _logger.LogInformation("[LanciaConsumer] LanciaConsumer");

                using (var canale = _mioRabbitConnessione.CreateModel())
                {
                    //creo la queue se non esistesse gia'
                    _logger.LogInformation("[LanciaConsumer] Creo la queue =" + nome_coda + " se non esistesse gia'");
                    canale.QueueDeclare(queue: nome_coda,
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    //creo un callback invocata dall' evento "arrivo nuovo messaggio"
                    var evento_consumer = new EventingBasicConsumer(canale);
                    evento_consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        messaggio = Encoding.UTF8.GetString(body);
                        _logger.LogInformation("[LanciaConsumer] Ricevuto messaggio ="+ messaggio);
                        
                    };

                    //avvio il consumer
                    _logger.LogInformation("[LanciaConsumer] Mi metto in ascolto di un messaggio sulla queue =" + nome_coda);
                    canale.BasicConsume(queue: nome_coda,
                                         autoAck: true,
                                         consumer: evento_consumer);


                }

                return Ok(new { successo = "Dalla coda=" + nome_coda + " ricevuto message=" + messaggio });


            }
            catch (Exception ex)
            {
                _logger.LogError("[LanciaConsumer] Eccezzione nel ricevere dalla coda=" + nome_coda + " un messaggio : " + ex.Message);
                return Ok(new { errore = "Errore : " + ex.Message });
            }

        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
