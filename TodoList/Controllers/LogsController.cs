using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32.SafeHandles;
using TodoList.DTOs;
using TodoList.Models;
using TodoList.Services.Log;
using TodoList.Services.PasswordHash;
using TodoList.Services.TokenGenerator;

namespace TodoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly ILogService _ilogService;
        public AuthsController(ILogService ilogService)
        {
            _ilogService = ilogService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] UserRequest userRequest)
        {
            var user = await _ilogService.Register(userRequest);
            if (user == null) return BadRequest("Username or Email already exis");
            return user;
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserRequest userRequest)
        {
            var accessToken = await _ilogService.Login(userRequest);
            return Ok(accessToken);
        }
    }
}
