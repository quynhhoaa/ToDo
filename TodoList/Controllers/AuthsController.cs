using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Schema;
using Microsoft.Win32.SafeHandles;
using TodoList.DTOs;
using TodoList.Models;
using TodoList.Services.UserService;
using TodoList.Services.PasswordHash;
using TodoList.Services.TokenGenerator;

namespace TodoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly AccessTokenGenerator _accessTokenGenerator;

        public AuthsController(IUserService userService, AccessTokenGenerator accessTokenGenerator)
        {
           _userService = userService;
            _accessTokenGenerator = accessTokenGenerator;
        }
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterRequest registerRequest)
        {
            var existUserByEmail = await _userService.GetByEmail(registerRequest.Email);
            if (existUserByEmail != null)
            {
                return BadRequest("Email already exist");
            }
            var existUserByUsername = await _userService.GetByUsername(registerRequest.Username);
            if (existUserByUsername != null)
            {
                return BadRequest("Username already exist");
            }
            var user = await _userService.Register(registerRequest);
            return Ok(user);
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _userService.GetByUsername(loginRequest.Usename);
            if (user == null)
            {
                return NotFound();
            }
            if (!_userService.CheckLogin(loginRequest))
            {
                return BadRequest();
            }
            string accessToken = _accessTokenGenerator.CreateToken(user);
            return Ok(accessToken);
        }
    }
}
