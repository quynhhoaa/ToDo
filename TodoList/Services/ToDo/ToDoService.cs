using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TodoList.DTOs;
using TodoList.Models;

namespace TodoList.Services.ToDo
{
    public class ToDoService : IToDoService
    {
        private readonly TodoDbContext _context;
        private readonly IMapper _mapper;

        public ToDoService(TodoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task AddNewTask(ToDoRequest toDo)
        {
            var item = _mapper.Map<ToDoRequest, Models.Todo>(toDo);
            item.Date= DateTime.Now;
            item.Status = 0;
            _context.Tasks.Add(item);
            await _context.SaveChangesAsync();
        }
        public async Task CompleteTask(List<Guid> taskIDs)
        {
            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                var task = await _context.Tasks.Where(c => taskIDs.Contains(c.Id)).ToListAsync();
                foreach (var x in task)
                {
                    x.Status = 1;
                }
                _context.UpdateRange(task);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
        }

        public async Task CompleteTask(Guid userId, ToDoRequest toDo, List<Guid> taskIDs)
        {
            var item = _context.Tasks.FirstOrDefault(x => x.UserId == userId);
            if (item != null)
            {
                item.CategoryId = toDo.CategoryId;
                item.Details = toDo.Details;
                item.Title = toDo.Title;
                item.Date = DateTime.Now;
                _context.Update(item);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTask(Guid taskId)
        {
            try
            {
                var item = _context.Tasks.Where(x => x.Id == taskId).FirstOrDefault();
                _context.Tasks.Remove(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public async Task DeleteTask(Guid userId, Guid taskId)
        {
            var item = _context.Tasks.FirstOrDefault(x => x.UserId == userId && x.Id == taskId);
            _context.Tasks.Remove(item);
            await _context.SaveChangesAsync();
        }
        public async Task EditTask(Guid userId, Guid taskid, ToDoRequest toDo)
        {
            var item = _context.Tasks.FirstOrDefault(x => x.UserId == userId && x.Id == taskid);
            if (item != null)
            {
                item.CategoryId = toDo.CategoryId;
                item.Details = toDo.Details;
                item.Title = toDo.Title;
                item.Date = DateTime.Now;
                _context.Update(item);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Todo> GetById(Guid taskId)
        {
            return await _context.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);
        }

        public Task<List<Todo>> GetTasks(Guid userID, FilterResquest filterRequest)
        {
            throw new NotImplementedException();
        }
    }
}
