using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Business.Messages;
using MassTransit;
using MassTransit.RabbitMqTransport;
using MassTransit.Util;
using Topshelf;

namespace DispatcherService
{
    class Program
    {
        static void Main(string[] args)
        {
            var uri = new Uri("http://localhost:12345/");
            HostFactory.Run(x =>
            {
                x.Service<HttpDispatcherService>(service =>
                {
                    service.ConstructUsing(name => new HttpDispatcherService(uri));
                    service.WhenStarted(disp => disp.Start());
                    service.WhenStopped(disp => disp.Stop());
                });
                x.SetServiceName("HttpDispatcherservice");
                x.SetDescription("Restrfull Dispatcher service that receives commands from portals");
                x.SetDisplayName("Dispatcher Service");

                x.StartManually();

            });
        }
        //static void Main(string[] args)
        //{

          
        //    HostFactory


        //  
    }
}
