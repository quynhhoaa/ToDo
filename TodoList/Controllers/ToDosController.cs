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
            var userId = Guid.Parse(HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).FirstOrDefault());
            var item = await _toDoService.GetById(taskId, userId);
            return Ok(item);
        }
        [HttpPost]
        public async Task<IActionResult> AddNewTask([FromBody] ToDoRequest toDo)
        {
            var userId = Guid.Parse(HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).FirstOrDefault());
            await _toDoService.AddNewTask(userId, toDo);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] ToDoRequest toDo, Guid taskId)
        {
            var userId = Guid.Parse(HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault());
            await _toDoService.EditTask(userId, taskId, toDo);
            return Ok(toDo);
        }
        [HttpPatch("task/complete")]
        public async Task<IActionResult> CompleteTask([FromBody] List<Guid> taskIDs)
        {
            var userId = Guid.Parse(HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).FirstOrDefault());
            await _toDoService.CompleteTask(userId, taskIDs);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteTask(Guid taskId)
        {
            var userId = Guid.Parse(HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).FirstOrDefault());
            await _toDoService.DeleteTask(userId, taskId);
            return Ok();
        }
    }
}
