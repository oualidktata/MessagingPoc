using System;
using System.Configuration;
using DispatcherService;
using MassTransit;
using Topshelf;
using TaskUtil = MassTransit.Util.TaskUtil;

namespace NestorService
{
    public class NestorService:
        ServiceControl
    {
        private IBusControl _busControl;
        public bool Start(HostControl hostControl)
        {
            _busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(ConfigurationManager.AppSettings["RabbitMqHost"]), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                cfg.ReceiveEndpoint(host, ConfigurationManager.AppSettings["UpdateVettingQueue"], e =>
                {
                    e.Consumer<UpdateVettingConsumer>();
                });
            });
            TaskUtil.Await(() => _busControl.StartAsync());
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
           _busControl?.Stop();
            return true;
        }
    }
}
