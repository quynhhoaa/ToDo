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
    public class LogsController : ControllerBase
    {
        private readonly ILog _ilog;
        public LogsController(ILog log)
        {
            _ilog = log;
        }
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] UserRequest userRequest)
        {
            User user = await _ilog.Register(userRequest);
            return user;
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] UserRequest userRequest)
        {
            string accessToken = await _ilog.Login(userRequest);
            return accessToken;
        }
    }
}
