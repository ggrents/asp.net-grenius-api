namespace grenius_api.Application.Models.Requests
{
    public class UserRequestDTO
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
