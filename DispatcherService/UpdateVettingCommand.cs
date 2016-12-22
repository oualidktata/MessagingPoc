using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Business.Messages;
using MassTransit;

namespace DispatcherService
{
    public class UpdateVettingCommand
    {
        private readonly Uri _serviceProvider;

        public UpdateVettingCommand(Uri serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Send(ISendEndpointProvider sendEndpointProvider,UpdateVettingRequest vettingRequest)
        {
            var endpoint = await sendEndpointProvider.GetSendEndpoint(_serviceProvider);
            await endpoint.Send(vettingRequest);
        }
    }
}
