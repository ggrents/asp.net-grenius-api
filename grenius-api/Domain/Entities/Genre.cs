using System.ComponentModel.DataAnnotations;

namespace grenius_api.Domain.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } 
        public List<Song>? Songs { get; set; }
    }
}
