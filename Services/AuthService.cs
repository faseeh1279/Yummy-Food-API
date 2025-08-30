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
                Username = loginDTO.Name,
                Email = loginDTO.Email
            };
            var userResult = await _authRepository.GetUserAsync(user); // returns a string or null

            

            if (userResult.HashedPassword != null)
            {
                var result = _tokenService.VerifyPassword(userResult.HashedPassword, loginDTO.Password);
                if (result)
                {
                    var accessToken = _tokenService.GenerateJSONWebToken(loginDTO);
                    var refreshToken = _tokenService.GenerateRefreshToken();

                    var token = new RefreshToken
                    {
                        UserId = userResult.Id,
                        Token = refreshToken,
                        Expires = DateTime.UtcNow.AddDays(7),  // <-- Expires after 7 days
                    };

                    await _authRepository.AddRefreshToken(token);

                    return new LoginResultDTO
                    {
                        Message = "Login Successful",
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
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
                    AccessToken = "null",
                    RefreshToken = "null"
                };
            }

        }


        public async Task<User> SignUp(SignUpDTO signUpDTO)
        {
            var hashedPassword = _tokenService.HashPassword(signUpDTO.Password); // Hash the password before saving it
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = signUpDTO.Name,
                Email = signUpDTO.Email,
                PhoneNumber = signUpDTO.PhoneNumber,
                HashedPassword = hashedPassword, // Store the hashed password
            };

            var result = await _authRepository.SignUp(user);
            return result;
        }

        public async Task<string> GenerateNewAccessToken(string refreshToken)
        {
            var result = await _authRepository.GetRefreshToken(refreshToken);
            if(result == null)
            {
                return "Invalid Refresh Token"; 
            }

            if(result.Expires < DateTime.UtcNow)
            {
                return "Refresh Token Expired, Login Again to Continue"; 
            }
            else
            {
                var userData = await _authRepository.GetUserDataByRefreshToken(result.Token);
                if(userData != null)
                {
                    var loginDTO = new LoginDTO
                    {
                        Name = userData.Username,
                        Email = userData.Email
                    }; 
                    var response = _tokenService.GenerateJSONWebToken(loginDTO);
                    return response; 
                }
                else
                {
                    return "Something went wrong! Try Again"; 
                }
            } 
        }
    }
}
