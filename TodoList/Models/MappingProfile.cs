using AutoMapper;
using TodoList.DTOs;

namespace TodoList.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Todo, ToDoRequest>();
            CreateMap<User, RegisterRequest>().ReverseMap();
            CreateMap<User, UserResponse>().ReverseMap();
        }
    }
}
