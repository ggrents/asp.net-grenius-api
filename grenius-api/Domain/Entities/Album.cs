namespace grenius_api.Domain.Entities
{
#pragma warning disable 
    public class Album
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public int ArtistId { get; set; }
        public AlbumType AlbumTypeId { get; set; }
        public Artist Artist { get; set; }
        public List<Song> Songs { get; set; }
    }
#pragma warning restore
}
