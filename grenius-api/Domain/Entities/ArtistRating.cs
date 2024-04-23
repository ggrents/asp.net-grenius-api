namespace grenius_api.Domain.Entities
{
    public class ArtistRating
    {
        public int Id { get; set; }
        public int ArtistId { get; set; }
        public long Count { get; set; }
        public Artist? Artist { get; set; }
    }
}
