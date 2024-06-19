using Microsoft.EntityFrameworkCore;
using Todos.MinimalAPI.Models;

namespace Todos.MinimalAPI.Data
{
    public class TodoContext(DbContextOptions<TodoContext> options) : DbContext(options)
    {
        public DbSet<Todo> Todos { get; set; }
    }
}
