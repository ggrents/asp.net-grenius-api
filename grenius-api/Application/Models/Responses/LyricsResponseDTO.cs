namespace grenius_api.Application.Models.Responses
{
    public class LyricsResponseDTO
    {
        public int Id { get; set; }
        public int SongId { get; set; }
        public int UserCreatedId { get; set; }
        public string? Text { get; set; }
        public List<AnnotationResponseDTO>? Annotations { get; set; }
    }
}
