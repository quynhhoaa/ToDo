using TodoList.DTOs;
using TodoList.Models;
using TodoList.Services.PasswordHash;
using TodoList.Services.TokenGenerator;

namespace TodoList.Services.Log
{
    public class Log : ILog
    {
        private readonly BCryptPasswordHash _bCryptPasswordHash;
        public static User user = new User();
        public readonly TodoDbContext _todoDbContext;
        public readonly AccessTokenGenerator _tokenGenerator;
        public Log(BCryptPasswordHash bCryptPasswordHash, TodoDbContext todoDbContext, AccessTokenGenerator tokenGenerator)
        {
            _bCryptPasswordHash = bCryptPasswordHash;
            _todoDbContext = todoDbContext;
            _tokenGenerator = tokenGenerator;
        }
        public async Task<string> Login(UserRequest userRequest)
        {
            var user = _todoDbContext.Users.SingleOrDefault(x => x.Username == userRequest.Username);
            if (user == null)
            {
                return null;
            }
            if (!_bCryptPasswordHash.VerifyPassword(userRequest.Password, user.PasswordHash))
            {
                return null;
            }
            string token = _tokenGenerator.CreateToken(user);
            return token;
        }
        public async Task<User> Register(UserRequest userRequest)
        {
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