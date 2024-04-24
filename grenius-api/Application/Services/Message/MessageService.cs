using MassTransit;
using MessageContracts;

namespace grenius_api.Application.Services.Message
{
    public class MessageService : IMessageService
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MessageService(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        public async Task Publish(RatingMessage message)
        {
            await _publishEndpoint.Publish(message);
        }
    }
}
