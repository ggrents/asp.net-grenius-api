namespace grenius_api.Application.Models.Requests
{
    public class SongUpdateRequestDTO
    {
        public string Title { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public bool IsFeature { get; set; }
        public int ArtistId { get; set; }
        public int? AlbumId { get; set; }
        public List<FeatureUpdateRequestDTO>? Features { get; set; }
    }
}
