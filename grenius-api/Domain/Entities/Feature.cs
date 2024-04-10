namespace grenius_api.Domain.Entities
{
#pragma warning disable
    public class Feature
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        public int SongId { get; set; }
        public int ArtistId { get; set; }
        public Song Song { get; set; }
        public Artist Artist { get; set; }
    }
#pragma warning restore
}
