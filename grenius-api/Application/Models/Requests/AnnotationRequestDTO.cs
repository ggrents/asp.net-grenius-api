namespace grenius_api.Application.Models.Requests
{
    public class AnnotationRequestDTO
    {
        public string Fragment { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
        public int LyricsId { get; set; }
    }
}

