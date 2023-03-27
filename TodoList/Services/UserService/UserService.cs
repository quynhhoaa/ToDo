using Microsoft.EntityFrameworkCore;
using TodoList.DTOs;
using TodoList.Models;
using TodoList.Services.PasswordHash;
using TodoList.Services.TokenGenerator;

namespace TodoList.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly BCryptPasswordHash _bCryptPasswordHash;
        public readonly TodoDbContext _todoDbContext;
        public readonly AccessTokenGenerator _tokenGenerator;
        public UserService(BCryptPasswordHash bCryptPasswordHash, TodoDbContext todoDbContext, AccessTokenGenerator tokenGenerator)
        {
            _bCryptPasswordHash = bCryptPasswordHash;
            _todoDbContext = todoDbContext;
            _tokenGenerator = tokenGenerator;
        }
        public async Task<UserResponse?> GetByEmail(string email)
        {
            var user = await _todoDbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
               return null;
            }
            return new UserResponse
            {
                Id = user.Id,
                Email = email,
                Username = user.Username
            };
        }
        public async Task<UserResponse?> GetByUsername(string username)
        {
            var user = await _todoDbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
            {
                return null;
            }
            else 
            {
                return new UserResponse
                {
                    Id = user.Id,
                    Username = username,
                    Email = user.Email
                };
            };  
        }
        public bool CheckLogin(LoginRequest loginRequest)
        {
           var check = _todoDbContext.Users.Where(x => x.Username == loginRequest.Usename).FirstOrDefault();
            return _bCryptPasswordHash.VerifyPassword(loginRequest.Password, check.PasswordHash);
        }
        public async Task<User> Register(RegisterRequest registerRequest)
        {
            var passwordHash = _bCryptPasswordHash.HashPassword(registerRequest.Password);
            var user = new User 
            {
                Username = registerRequest.Username,
                PasswordHash = passwordHash,
                Email = registerRequest.Email,
                Id = new Guid()
            };
            _todoDbContext.Add(user);
            await _todoDbContext.SaveChangesAsync();
            return user;
        }
    }
}
