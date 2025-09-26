using Microsoft.EntityFrameworkCore;
using Sharoo.Server.Domain.Entities;

namespace Sharoo.Server.Data.Repositories.Todos
{
    public class TodoRepository : ITodoRepository
    {
        private readonly SharooDbContext _context;

        public TodoRepository(SharooDbContext context)
        {
            _context = context;
        }

        public async Task ChangeStatusAsync(Todo todo)
        {
            todo.IsDone = !todo.IsDone;
            _context.Todos.Update(todo);

            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(Todo todo)
        {
            await _context.Todos.AddAsync(todo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Todo todo)
        {
            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Todo>> ReadAsync()
        {
            return await _context.Todos
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Todo?> ReadByIdAsync(Guid todoId)
        {
            return await _context.Todos
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == todoId);
        }
    }
}
