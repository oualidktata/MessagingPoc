using System;
using System.Configuration;
using System.Diagnostics;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.SelfHost;
using MassTransit;
using MassTransit.RabbitMqTransport;
using MassTransit.Util;

namespace DispatcherService
{
    public class HttpDispatcherService
    {
        private readonly HttpSelfHostServer _server;
        private readonly HttpSelfHostConfiguration _config;
        private readonly string EventSource = "HttpDispatcherService";
        private static IRabbitMqHost _host;

        public HttpDispatcherService(Uri baseAddress)
        {
            if (!EventLog.SourceExists(EventSource))
            {
                EventLog.CreateEventSource(EventSource,"Application");
            }
            EventLog.WriteEntry(EventSource,$"Creating server at {baseAddress}");

            _config = new HttpSelfHostConfiguration(baseAddress);
            _config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            _config.Routes.MapHttpRoute(name: "defaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional });
            _server = new HttpSelfHostServer(_config);
        }

        public void Start()
        {
            EventLog.WriteEntry(EventSource,"Opening Http Web service");
            _server.OpenAsync();
            var busControl = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                _host = x.Host(new Uri(ConfigurationManager.AppSettings["RabbitMQHost"]), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });

            TaskUtil.Await(() => busControl.StartAsync());
        }

        public void Stop()
        {
            _server.CloseAsync().Wait();
            _server.Dispose();
        }
    }
}