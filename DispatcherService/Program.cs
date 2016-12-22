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


        //  var updateVettingQueueUri = new Uri(ConfigurationManager.AppSettings["RabbitMQHost"] + ConfigurationManager.AppSettings["UpdateVettingQueue"]);

        //    var command= new UpdateVettingCommand(updateVettingQueueUri);


        //    try
        //    {
        //        var client =busControl.CreateRequestClient<UpdateVettingRequest, UpdateVettingResponse>(updateVettingQueueUri,TimeSpan.FromSeconds(10));



        //        for (;;)
        //        {
        //            Console.Write("Send vetting Status, Press Q to exit:");
        //            var vettingStatus = Console.ReadLine();
        //            if (vettingStatus == "Q")
        //            {
        //                break;
        //            }
        //            if (string.IsNullOrEmpty(vettingStatus))
        //            {
        //                vettingStatus = "AskingForVetting";
        //            }
        //            Console.WriteLine("Sending {0}", vettingStatus);
        //            var updateMessageCommand = new UpdateVettingRequest
        //            {
        //                NestorNumber = $"16-2002{vettingStatus}",
        //                VettingStatus = $"Status{vettingStatus}"
        //            };

        //            Task.Run(async () =>
        //            {
        //                var response = await client.Request(updateMessageCommand);
        //                Console.WriteLine(
        //                $"RESPONSE: SUCCESS ={response.Success}"
        //                );
        //            }).Wait();


        //            //Task.Run(async () =>
        //            //{
        //            //    await command.Send(busControl, updateMessageCommand);
        //            //}).Wait();

        //            //});
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);

        //    }
        //    finally
        //    {
        //        busControl?.Stop();
        //    }
        //}
    }
}
