using TodoList.DTOs;

namespace TodoList.Services.ToDo
{
    public interface IToDoService
    {
        Task<List<Models.Todo>> GetTasks(Guid userID, FilterResquest filterRequest);
        Task<Models.Todo> GetById(Guid taskid, Guid userId);
        Task AddNewTask(Guid userId, ToDoRequest toDo);
        Task EditTask(Guid userId, Guid taskId, ToDoRequest toDo);
        Task<Response> DeleteTask(Guid userId, Guid taskId);
        Task CompleteTask(Guid userId, List<Guid> taskIDs);
    }
}
