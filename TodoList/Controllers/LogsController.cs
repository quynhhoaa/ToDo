using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32.SafeHandles;
using TodoList.DTOs;
using TodoList.Models;
using TodoList.Services.PasswordHash;
using TodoList.Services.TokenGenerator;

namespace TodoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        public static User user = new User();
        private readonly BCryptPasswordHash _bCryptPasswordHash;
        private readonly TodoDbContext _todoDbContext;
        private readonly AccessTokenGenerator _tokenGenerator;

        public LogsController(BCryptPasswordHash bCryptPasswordHash, TodoDbContext todoDbContext, AccessTokenGenerator tokenGenerator)
        {
            _bCryptPasswordHash = bCryptPasswordHash;
            _todoDbContext = todoDbContext;
            _tokenGenerator = tokenGenerator;
        }
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] UserRequest userRequest)
        {
            var passwordHash = _bCryptPasswordHash.HashPassword(userRequest.Password);
            user.Username = userRequest.Username;
            user.PasswordHash = passwordHash;
            user.Email = userRequest.Email;
            user.Id = new Guid();
            _todoDbContext.Add(user);
            await _todoDbContext.SaveChangesAsync();
            return Ok(user);
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] UserRequest userRequest)
        {
            var user = _todoDbContext.Users.SingleOrDefault(x => x.Username == userRequest.Username);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            /* var passwordHash = _bCryptPasswordHash.HashPassword(userRequest.Password);
             var password = _todoDbContext.Users.Where(x => x.PasswordHash == passwordHash);*/
            if (!_bCryptPasswordHash.VerifyPassword( userRequest.Password, user.PasswordHash))
            {
                return BadRequest("Wrong password");
            }
            /*if (password == null)
            {
                return BadRequest("Password not found");
            }*/
            
            string token = _tokenGenerator.CreateToken(user);
            return Ok(token);
        }
    }
}
