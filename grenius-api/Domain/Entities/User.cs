namespace grenius_api.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required byte[] PasswordHash { get; set; } 
        public required string Email { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public List<UserRole>? UserRoles { get; set; }
        public List<Lyrics>? Lyrics { get; set; }
        public List<Annotation>? Annotations { get; set; }

    }
}
