using TodoList.DTOs;
using TodoList.Models;
using TodoList.Services.PasswordHash;
using TodoList.Services.TokenGenerator;

namespace TodoList.Services.Log
{
    public class LogService : ILogService
    {
        private readonly BCryptPasswordHash _bCryptPasswordHash;
        public readonly TodoDbContext _todoDbContext;
        public readonly AccessTokenGenerator _tokenGenerator;
        public LogService(BCryptPasswordHash bCryptPasswordHash, TodoDbContext todoDbContext, AccessTokenGenerator tokenGenerator)
        {
            _bCryptPasswordHash = bCryptPasswordHash;
            _todoDbContext = todoDbContext;
            _tokenGenerator = tokenGenerator;
        }
        public async Task<string> Login(UserRequest userRequest)
        {
            string message = "";
            var user = _todoDbContext.Users.SingleOrDefault(x => x.Username == userRequest.Username);
            if (user == null)
            {
                message = "Username is invalid";
                return message;
            }
            if (!_bCryptPasswordHash.VerifyPassword(userRequest.Password, user.PasswordHash))
            {
                message = "Password is invalid";
                return message;
            }
            string token = _tokenGenerator.CreateToken(user);
            return token;
        }
        public async Task<User> Register(UserRequest userRequest)
        {
            var model = _todoDbContext.Users.SingleOrDefault(x => x.Email == userRequest.Email || x.Username == userRequest.Username);
            if(model != null) { return null; }
            User user = new User();
            var passwordHash = _bCryptPasswordHash.HashPassword(userRequest.Password);
            user.Username = userRequest.Username;
            user.PasswordHash = passwordHash;
            user.Email = userRequest.Email;
            user.Id = new Guid();
            _todoDbContext.Add(user);
            await _todoDbContext.SaveChangesAsync();
            return user;
        }
    }
}