using grenius_rating.Application.Repository;
using MassTransit;
using MessageContracts;

namespace grenius_rating.Application.Consumers
{
    public class RatingConsumer : IConsumer<RatingMessage>
    {
        private readonly IDataRepository _repo;
        public RatingConsumer(IDataRepository repository)
        {
            _repo = repository;
        }
        public async Task Consume(ConsumeContext<RatingMessage> context)
        {
            await _repo.AddRatingCount(context.Message.EntityId, (int)context.Message.Type);
        }
    }
}
