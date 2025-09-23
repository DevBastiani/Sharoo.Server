using Microsoft.Extensions.DependencyInjection;
using Sharoo.Server.Data.Repositories.Todos;

namespace Sharoo.Server.Data
{
    public static class DataInitializer
    {
        public static void AddDependencyInjections(this IServiceCollection services)
        {
            services.AddScoped<ITodoRepository, TodoRepository>();
        }
    }
}
