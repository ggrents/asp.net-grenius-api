namespace grenius_api.Domain.Entities
{

    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public DateTime Birthday { get; set; }
        public List<Song>? Songs { get; set; }
        public List<Feature>? Features { get; set; }
    }

}
