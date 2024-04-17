using grenius_api.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace grenius_api.Application.Services
{
    public class UserService : IUserService
    {
        private readonly SecurityOptions _options;
        private const int MinPasswordLength = 6; 
        private const string SpecialCharacters = "!@#$%^&*()-_=+";
        private readonly HMACSHA512 _hmac;
        private readonly SymmetricSecurityKey secretKey;
        private double _expirationHours;

        public UserService(IOptions<SecurityOptions> options)
        {
            _options = options.Value;
            _expirationHours = _options.ExpirationHours;
            secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
            _hmac = new HMACSHA512(Encoding.UTF8.GetBytes(_options.Secret));
        }
        public bool IsPasswordValid(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            if (password.Length < MinPasswordLength)
                return false;

            if (!password.Any(char.IsDigit))
                return false;

            // Hidden for simplification 

            //if (!password.Any(char.IsUpper))
            //    return false;


            //if (!password.Any(char.IsLower))
            //    return false;

            //if (!password.Any(c => SpecialCharacters.Contains(c)))
            //    return false;

            return true;
        }

        public bool ValidatePassword(string password, byte[] hashedPassword)
        {
            var computedHash = HashPassword(password); 
            return computedHash.SequenceEqual(hashedPassword);
        }

        public byte[] HashPassword(string password)
        {
            var computedHash = _hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash;
        }

        public string CreateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
           {
                new Claim("userId", user.Id.ToString()),
                //new Claim(ClaimTypes.Name, user.Username),
                //new Claim("roleId", user.)
           }),
                Expires = DateTime.UtcNow.AddHours(_expirationHours),
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
