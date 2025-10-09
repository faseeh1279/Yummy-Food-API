using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using Yummy_Food_API.Models.DTOs;
using Yummy_Food_API.Services.Interfaces;

namespace Yummy_Food_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDTO signUpDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.SignUp(signUpDTO);
                if (result.Success)
                    return Ok(result.Data);
                return BadRequest(result.Message); 
            }
            else
            {
                return BadRequest("All Fields are required"); 
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.Login(loginDTO);
                if (result.Success)
                    return Ok(result.Data);
                return BadRequest(result.Message); 
            }
            return BadRequest(ModelState);
        }

        [AllowAnonymous]
        [HttpPost("Generate-Access-Token")]
        public async Task<IActionResult> GenerateAccessToken([FromBody] RefreshTokenDTO refreshTokenDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.GenerateNewAccessToken(refreshTokenDTO.accessToken);
                if (result.Success)
                    return Ok(result.Data);
                return BadRequest(result.Message); 
            }
            return BadRequest(ModelState); 
        }
    }
}
