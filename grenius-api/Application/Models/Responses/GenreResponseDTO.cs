using System.ComponentModel.DataAnnotations;

namespace grenius_api.Application.Models.Responses
{
    public class GenreResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
