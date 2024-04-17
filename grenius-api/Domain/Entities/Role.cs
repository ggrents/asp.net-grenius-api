namespace grenius_api.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<UserRole>? UserRoles { get; set; }
    }
}
