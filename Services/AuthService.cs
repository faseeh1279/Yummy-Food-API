using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
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

        public async Task<ServiceResponse<LoginResultDTO>> Login(LoginDTO loginDTO)
        {
            var response = new ServiceResponse<LoginResultDTO>();
            var user = await _authRepository.GetUserAsync(loginDTO.Email);
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }
            if (user.Username != loginDTO.Username)
            {
                response.Success = false;
                response.Message = "Invalid Credentials";
                return response;
            }
            var result = _tokenService.VerifyPassword(user.HashedPassword, loginDTO.Password);

            if (!result)
            {
                response.Success = false;
                response.Message = "Invalid Credentials";
                return response;
            }

            var accessToken = await _tokenService.GenerateJSONWebToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var token = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                Expires = DateTime.UtcNow.AddDays(7),
            };
            await _authRepository.AddRefreshToken(token);
            response.Success = true;
            response.Data = new LoginResultDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            return response;


        }

        public async Task<ServiceResponse<User>> SignUp(SignUpDTO signUpDTO)
        {
            var response = new ServiceResponse<User>();
            var hashedPassword = _tokenService.HashPassword(signUpDTO.Password); // Hash the password before saving it
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = signUpDTO.Username,
                Email = signUpDTO.Email,
                PhoneNumber = signUpDTO.PhoneNumber,
                HashedPassword = hashedPassword, // Store the hashed password
            };

            var result = await _authRepository.SignUp(user);
            if (result == null)
            {
                response.Success = false;
                response.Message = "User with this email already exists.";
                return response;
            }
            response.Success = true;
            response.Data = result;
            return response;
        }

        public async Task<ServiceResponse<string>> GenerateNewAccessToken(string refreshToken)
        {
            var response = new ServiceResponse<string>();
            var result = await _authRepository.GetRefreshToken(refreshToken);
            if (result == null)
            {
                response.Success = false;
                response.Message = "Invalid Refresh Token";
                return response;
            }

            if (result.Expires < DateTime.UtcNow)
            {
                response.Success = false;
                response.Message = "Refresh Token Expired, Login Again to Continue";
                return response; 
            }
            else
            {
                var user = await _authRepository.GetUserAsync(result.User.Email);
                if(user== null)
                {
                    response.Success = false;
                    response.Message = "Something went wrong! Try Again";
                    return response;
                }
                response.Success = true;
                response.Data = await _tokenService.GenerateJSONWebToken(new User
                {
                    Username = user.Username,
                    Email = user.Email
                });
                return response; 
            }
        }
    }
}
