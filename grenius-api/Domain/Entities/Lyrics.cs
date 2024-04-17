namespace grenius_api.Domain.Entities
{
    public class Lyrics
    {
        public int Id { get; set; }
        public int SongId { get; set; }
        public int UserCreatedId { get; set; }
        public string? Text { get; set; }
        public Song? Song { get; set; }
        public User? UserCreated { get; set; }
        public List<Annotation>? Annotations { get; set; }
    }
}
