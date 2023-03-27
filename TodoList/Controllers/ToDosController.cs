using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoList.DTOs;
using TodoList.Services.ToDo;

namespace TodoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDosController : ControllerBase
    {
        private readonly IToDoService _toDoService;

        public ToDosController(IToDoService toDoService)
        {
            _toDoService = toDoService;
        }
        [HttpGet]
        [Route("allTask")]
        public async Task<IActionResult> GetAllTask([FromQuery] FilterResquest filter)
        {
            var userId = Guid.Parse(HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).FirstOrDefault());
            return Ok(await _toDoService.GetTasks(userId, filter));
        }
        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetById(Guid taskId)
        {
            return Ok(await _toDoService.GetById(taskId));
        }
        [HttpPost]
        public async Task<IActionResult> AddNewTask([FromBody]ToDoRequest toDo)
        {
            await _toDoService.AddNewTask(toDo);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> EditTask(Guid userId,Guid taskId, ToDoRequest toDo)
        {
            await _toDoService.EditTask(userId, taskId, toDo);
            return Ok(toDo);
        }
        [HttpPost]
        [Route("task/complete")]
        public async Task<IActionResult> CompleteTask(Guid userId, ToDoRequest toDo, List<Guid> taskIDs)
        {
            await _toDoService.CompleteTask(userId, toDo, taskIDs);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteTask(Guid userId, Guid taskId)
        {
            await _toDoService.DeleteTask(userId, taskId);
            return Ok();
        }
    }
}
