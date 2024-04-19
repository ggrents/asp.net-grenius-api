
namespace grenius_api.Application.Models.Responses
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public int RoleId { get; set; }
     
    }
}
