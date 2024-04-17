using grenius_api.Domain.Entities;

namespace grenius_api.Application.Services
{
    public interface IUserService
    {
        public bool IsPasswordValid(string password);
        public bool ValidatePassword(string password, byte[] hashedPassword);
        public byte[] HashPassword(string password);
        public string CreateToken(User user);
    }
}
