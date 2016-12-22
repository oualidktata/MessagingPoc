using System;
using System.Threading.Tasks;
using Core.Business.Messages;
using MassTransit;

namespace NestorService
{
    public class UpdateVettingConsumer:IConsumer<UpdateVettingRequest>
    {
        public async Task Consume(ConsumeContext<UpdateVettingRequest> context)
        {
            await Console.Out.WriteLineAsync(
                $"Updating vetting: {context.Message.NestorNumber}-{context.Message.VettingStatus}");
            context.Respond(new UpdateVettingResponse{Success = $"{context.Message.NestorNumber}-{context.Message.VettingStatus} has been Succefully Handled "});
        }
    }
}
