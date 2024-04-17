namespace grenius_api.Domain.Entities
{
    public class Annotation
    {
        public int Id { get; set; }
        public int StartSymbol { get; set; }
        public int EndSymbol { get; set; }
        public string Text { get; set; } = string.Empty;
        public int LyricsId { get; set; }
        public int UserCreatedId { get; set; }
        public Lyrics Lyrics { get; set; }
        public User? UserCreated { get; set; }
    }
}
