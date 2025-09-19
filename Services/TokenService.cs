using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;
using Yummy_Food_API.Repositories.Interfaces;
using Yummy_Food_API.Services.Interfaces;

namespace Yummy_Food_API.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly IAuthRepository _authRepository;
        public TokenService(IConfiguration config, IAuthRepository authRepository)
        {
            _config = config;
            _authRepository = authRepository;
        }

        public async Task<string> GenerateJSONWebToken(LoginDTO loginDTO)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var users = await _authRepository.GetUsersAsync();
            var user = users.FirstOrDefault(u => u.Username == loginDTO.Username
                                  && u.Email == loginDTO.Email);




            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, loginDTO.Username),
                new Claim(JwtRegisteredClaimNames.Email, loginDTO.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user!.Role.ToString())
                };
            var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials
        );



            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        
        public string HashPassword(string password)
        {
            var hasher = new PasswordHasher<object>();
            return hasher.HashPassword(null, password);
        }

        public bool VerifyPassword(string hashedPassword, string enteredPassword)
        {
            var hasher = new PasswordHasher<object>();
            var result = hasher.VerifyHashedPassword(null, hashedPassword, enteredPassword);
            return result == PasswordVerificationResult.Success;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
