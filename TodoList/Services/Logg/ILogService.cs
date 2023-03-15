using TodoList.DTOs;
using TodoList.Models;

namespace TodoList.Services.Log
{
    public interface ILogService
    {
        Task<User> Register(UserRequest userRequest);
        Task<string> Login(UserRequest userRequest);
    }
}