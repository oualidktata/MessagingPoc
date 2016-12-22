using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Core.Business.Messages;
using MassTransit;
using MassTransit.Util;

namespace DispatcherService
{
    public class DemoController:ApiController
    {
        [HttpGet]
        public IHttpActionResult Index()
        {

            var updateVettingQueueUri = new Uri(ConfigurationManager.AppSettings["RabbitMQHost"] + ConfigurationManager.AppSettings["UpdateVettingQueue"]);
            var busControl = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                var _host = x.Host(new Uri(ConfigurationManager.AppSettings["RabbitMQHost"]), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });

            TaskUtil.Await(() => busControl.StartAsync());
            var command = new UpdateVettingCommand(updateVettingQueueUri);


            try
            {
                var vettingStatus = new Random(12);
                var client =
                    busControl.CreateRequestClient<UpdateVettingRequest, UpdateVettingResponse>(updateVettingQueueUri,
                        TimeSpan.FromSeconds(10));
                var updateMessageCommand = new UpdateVettingRequest
                {
                    NestorNumber = $"16-2002{vettingStatus}",
                    VettingStatus = $"Status{vettingStatus}"
                };

                Task.Run(async () =>
                {
                    var response = await client.Request(updateMessageCommand);
                    Console.WriteLine(
                    $"RESPONSE: SUCCESS ={response.Success}"
                    );
                }).Wait();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception: {exception.Message}");
            }
            finally
            {
                busControl?.Stop();
            }

            return Ok("Hello World");
        }
    }
}
