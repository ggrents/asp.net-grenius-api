using grenius_api.Domain.Entities;

namespace grenius_api.Application.Models.Responses
{
    public class AlbumResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public int ArtistId { get; set; }
        public string AlbumType { get; set; } = string.Empty;

    }
}
