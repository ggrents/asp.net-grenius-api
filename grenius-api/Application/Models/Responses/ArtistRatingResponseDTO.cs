using grenius_api.Domain.Entities;

namespace grenius_api.Application.Models.Responses
{
    public class ArtistRatingResponseDTO
    {
        public int Id { get; set; }
        public required Artist Artist { get; set; }
        public long Count { get; set; }
    }
}
