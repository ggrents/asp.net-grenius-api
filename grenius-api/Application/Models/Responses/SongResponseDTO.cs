namespace grenius_api.Application.Models.Responses
{
    public class SongResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public bool IsFeature { get; set; }
        public int ArtistId { get; set; }
        public int? GenreId { get; set; }
        public int? ProducerId { get; set; }
        public int? AlbumId { get; set; }
        public List<FeatureResponseDTO>? Features { get; set; }
    }
}
