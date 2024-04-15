namespace grenius_api.Domain.Entities
{
#pragma warning disable
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } 
        public DateTime ReleaseDate { get; set; }
        public bool IsFeature { get; set; }
        public int ArtistId { get; set; }
        public int? AlbumId { get; set; }
        public int? ProducerId { get; set; }
        public int? GenreId { get; set; }
        public Artist Artist { get; set; }
        public Album Album{ get; set; }
        public Genre Genre{ get; set; }
        public Producer Producer{ get; set; }
        public List<Feature> Features { get; set; }
    }
#pragma warning restore
}
