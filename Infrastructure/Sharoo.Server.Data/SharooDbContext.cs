using Microsoft.EntityFrameworkCore;
using Sharoo.Server.Domain.Entities;

namespace Sharoo.Server.Data
{
    public class SharooDbContext : DbContext
    {
        public SharooDbContext(DbContextOptions<SharooDbContext> options) : base(options) { }

        public DbSet<Todo> Todos { get; set; }
    }
}
