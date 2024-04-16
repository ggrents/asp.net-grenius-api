namespace grenius_api.Application.Models.Responses
{
    public class AnnotationResponseDTO
    {
        public int Id { get; set; }
        public string Fragment { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
        public int LyricsId { get; set; }
    }
}
