namespace grenius_api.Application.Models.Requests
{
    public class FeatureUpdateRequestDTO
    {
        public int? Id { get; set; }
        public int Priority { get; set; }
        public int ArtistId { get; set; }
    }
}
