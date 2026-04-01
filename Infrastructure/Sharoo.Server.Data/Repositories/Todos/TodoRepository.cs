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

        public async Task<List<Todo>> ReadAsync(Guid userId)
        {
            return await _context.Todos
                .Where(todo => todo.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Todo>> ReadByFilterAsync(
            Guid userId,
            DateTime? createdFrom,
            DateTime? createdTo,
            DateTime? completedFrom,
            DateTime? completedTo)
        {
            var query = _context.Todos.Where(todo => todo.UserId == userId);

            if (createdFrom.HasValue)
                query = query.Where(todo => todo.CreatedAt >= createdFrom.Value);

            if (createdTo.HasValue)
                query = query.Where(todo => todo.CreatedAt <= createdTo.Value);

            if (completedFrom.HasValue)
                query = query.Where(todo => todo.CompletedAt.HasValue && todo.CompletedAt >= completedFrom.Value);

            if (completedTo.HasValue)
                query = query.Where(todo => todo.CompletedAt.HasValue && todo.CompletedAt <= completedTo.Value);

            return await query.ToListAsync();
        }

        public async Task<Todo?> ReadByIdAsync(Guid todoId)
        {
            return await _context.Todos
                .FirstOrDefaultAsync(todo => todo.Id == todoId);
        }
    }
}
