using MessageContracts;

namespace grenius_api.Application.Services.Message
{
    public interface IMessageService
    {
        Task Publish(RatingMessage message);
    }
}
