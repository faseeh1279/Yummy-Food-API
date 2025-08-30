using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDTO signUpDTO)
        {
            var result = await _authService.SignUp(signUpDTO);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var result = await _authService.Login(loginDTO);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("Generate-Access-Token")]
        public async Task<IActionResult> GenerateAccessToken([FromBody] string refreshToken)
        {
            var result = await _authService.GenerateNewAccessToken(refreshToken);
            return Ok(result);
        }
    }
}
