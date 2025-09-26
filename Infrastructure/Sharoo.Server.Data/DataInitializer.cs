using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sharoo.Server.Data.Repositories.Todos;

namespace Sharoo.Server.Data
{
    public static class DataInitializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITodoRepository, TodoRepository>();
        }

        public static void InitializeDatabase(this IServiceCollection services)
        {
            services.AddDbContext<SharooDbContext>(options => options.UseInMemoryDatabase("SharooDb"));
        }
    }
}
