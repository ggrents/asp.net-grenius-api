namespace grenius_api.Domain.Entities
{
    public class SongRating
    {
        public int Id { get; set; }
        public int SongId { get; set; }
        public long Count { get; set; }
        public Song? Song { get; set; }
    }
}
