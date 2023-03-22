using TodoList.DTOs;
using TodoList.Models;

namespace TodoList.Services.UserService
{
    public interface IUserService
    {
        Task<UserResponse?> GetByEmail(string email);
        Task<UserResponse?> GetByUsername(string username);
        bool CheckLogin(LoginRequest loginRequest);
        Task<User> Register(RegisterRequest registerRequest);
    }
}
