namespace grenius_api.Application.Models.Requests
{
    public class UserRegisterRequestDTO
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
