using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Yummy_Food_API.Enums;
using Yummy_Food_API.Models.Domain;
using Yummy_Food_API.Models.DTOs;
using Yummy_Food_API.Repositories.Interfaces;
using Yummy_Food_API.Services.Interfaces;


namespace Yummy_Food_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenService _tokenService;
        public AuthService(IAuthRepository authRepository, ITokenService tokenService)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
        }

        public async Task<LoginResultDTO> Login(LoginDTO loginDTO)
        {
            var user = new User
            {
                Name = loginDTO.Name,
                Email = loginDTO.Email
            };
            var hashedPassword = await _authRepository.GetUserAsync(user); // returns a string or null
            if (hashedPassword != null)
            {
                var result = _tokenService.VerifyPassword(hashedPassword, loginDTO.Password);
                if (result)
                {
                    var accessToken = _tokenService.GenerateJSONWebToken(loginDTO);
                    //var refreshToken = _tokenService.GenerateRefreshToken(); 
                    return new LoginResultDTO
                    {
                        Message = "Login Successful",
                        AccessToken = accessToken,
                        RefreshToken = ""
                    };
                }
                else
                {
                    return new LoginResultDTO
                    {
                        Message = "Invalid Password",
                        AccessToken = "null",
                        RefreshToken = "null"
                    };
                }
               
            }
            else
            {
                return new LoginResultDTO
                {
                    Message = "Login Failed",
                    AccessToken = "AccessToken",
                    RefreshToken = "RefreshToken"
                };
            }

        }


        public async Task<User> SignUp(SignUpDTO signUpDTO)
        {
            var hashedPassword = _tokenService.HashPassword(signUpDTO.Password); // Hash the password before saving it
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = signUpDTO.Name,
                Email = signUpDTO.Email,
                PhoneNumber = signUpDTO.PhoneNumber,
                HashedPassword = hashedPassword, // Store the hashed password
            };

            var result = await _authRepository.SignUp(user);
            return result;
        }

    }
}
