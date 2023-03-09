using Microsoft.EntityFrameworkCore;

namespace TodoList.Models
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext() { }
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Todo> Tasks { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
