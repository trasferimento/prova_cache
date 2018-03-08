using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using StackExchange.Redis;  // per Redis Cache
using RabbitMQ.Client; // per RabbitMQ messages' broker

namespace prova_cache_redis
{
    public class Startup
    {

        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            //            Configuration = configuration;
            //aggiungo da appsettings.json
            //aggiungo da variabili ambiente
            var configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
           
            Configuration = configBuilder.Build();

            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<Startup>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation("[StartUp] Aggiungo come servizio MVC ");
            services.AddMvc();


     
            if (Configuration["Redis:enabled"] == "yes")
            { 
            try
            {

                //se in appsetting ho messo 
                //"Redis": {
                //    "Server": "redis:6379",
                //    "Password": "password"
                //}
                string redis_server= Configuration["Redis:Server"]; 
                string redis_pwd = Configuration["Redis:Password"];
                string connection_string = redis_server + ",password=" + redis_pwd;
                //string connection_string = "redis:6379,password=password,name=sono_il_client_1";
                _logger.LogInformation("Aggiungo come servizio Redis,e mi connetto con : " + connection_string);
                ConnectionMultiplexer mio_client_redis = ConnectionMultiplexer.Connect(connection_string); //qui si prova a connettersi !!! mettere TRY !!
               services.AddSingleton(mio_client_redis as IConnectionMultiplexer);
            }
            catch ( Exception ex)
            {
                _logger.LogError("[StartUp] Eccessione nel connettermi a Redis : " + ex.Message);
            }

            }

            if (Configuration["RabbitMQ:enabled"] == "yes")
            {
                try
                {
                    string UserName = Configuration["RabbitMQ:UserName"];
                    string Password = Configuration["RabbitMQ:Password"];
                    string VirtualHost = Configuration["RabbitMQ:VirtualHost"];
                    string HostName = Configuration["RabbitMQ:HostName"];
                    int Port = Convert.ToInt32( Configuration["RabbitMQ:Port"] );

                    //string connection_string = "amqp://user:pass@hostName:port/vhost"; 
                    string connection_string = "amqp://" + UserName + ":" + Password + "@" + HostName + ":" + Port + "/" + VirtualHost;
                    _logger.LogInformation("Aggiungo come servizio RabbitMq,e mi connetto con : " + connection_string);

                    ConnectionFactory factory = new ConnectionFactory();
                    factory.VirtualHost = UserName;
                    factory.Password = Password;
                    factory.VirtualHost = VirtualHost;
                    factory.HostName = HostName;
                    factory.Port = Port;

                    IConnection connessione = factory.CreateConnection();  //prova a connettersi
                    services.AddSingleton<IConnection>(connessione);

                    _logger.LogInformation("RabbitMq connesso !! ");
                }
                catch (Exception ex)
                {
                    _logger.LogError("[StartUp] Eccezzione nel connettermi a RabbitMQ : " + ex.Message);
                }
            }
                      

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
               // app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
