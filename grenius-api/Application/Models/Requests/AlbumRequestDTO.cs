using grenius_api.Domain.Entities;

namespace grenius_api.Application.Models.Requests
{
    public class AlbumRequestDTO
    {
        public string Title { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public int ArtistId { get; set; }
        public int AlbumType { get; set; } 
    }
}
