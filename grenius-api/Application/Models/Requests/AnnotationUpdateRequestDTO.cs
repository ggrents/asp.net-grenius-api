namespace grenius_api.Application.Models.Requests
{
    public class AnnotationUpdateRequestDTO
    {
        public int StartSymbol { get; set; }
        public int EndSymbol { get; set; }
        public string Text { get; set; } = string.Empty;
        public int LyricsId { get; set; }
    }
}
