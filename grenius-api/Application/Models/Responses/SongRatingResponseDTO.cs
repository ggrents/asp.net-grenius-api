using grenius_api.Domain.Entities;

namespace grenius_api.Application.Models.Responses
{
    public class SongRatingResponseDTO
    {
        public int Id { get; set; }
        public required Song Song { get; set; }
        public long Count { get; set; }
    }
}
