namespace grenius_api.Application.Models.Requests
{
    public class UserLoginRequestDTO
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
