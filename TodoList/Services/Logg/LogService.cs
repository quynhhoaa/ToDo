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
        public Task<UserResponse> GetByEmail(string email)
        {
            var user = _todoDbContext.Users.FirstOrDefault(x => x.Email == email);
            UserResponse userResponse = new UserResponse();
            if (user == null)
            {
                userResponse = null;
            }    
            else
            {
                userResponse.Id = user.Id;
                userResponse.Email = email;
                userResponse.Username = user.Username;
            }    
            return Task.FromResult(userResponse);
        }
        public Task<UserResponse> GetByUsername(string username)
        {
            var user = _todoDbContext.Users.FirstOrDefault(x => x.Username == username);
            UserResponse userResponse = new UserResponse();
            if (user == null)
            {
                userResponse = null;
            }    
            else
            {
                userResponse.Id = user.Id;
                userResponse.Username = username;
                userResponse.Email = user.Email;
            }    
            return Task.FromResult(userResponse);
        }
        public bool CheckLogin(LoginRequest loginRequest)
        {
            var check = _todoDbContext.Users.Where(x => x.Username == loginRequest.Usename).FirstOrDefault();
            return _bCryptPasswordHash.VerifyPassword(loginRequest.Password, check.PasswordHash);
        }
        public async Task<User> Register(RegisterRequest registerRequest)
        {
            var model = _todoDbContext.Users.SingleOrDefault(x => x.Email == registerRequest.Email || x.Username == registerRequest.Username);
            
            var user = new User();
            var passwordHash = _bCryptPasswordHash.HashPassword(registerRequest.Password);
            user.Username = registerRequest.Username;
            user.PasswordHash = passwordHash;
            user.Email = registerRequest.Email;
            user.Id = new Guid();
            _todoDbContext.Add(user);
            await _todoDbContext.SaveChangesAsync();
            return user;
        }
    }
}