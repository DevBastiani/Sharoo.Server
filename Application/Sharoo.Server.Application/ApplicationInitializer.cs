using Microsoft.Extensions.DependencyInjection;
using Sharoo.Server.Application.Services.Todos;
using Sharoo.Server.Data;

namespace Sharoo.Server.Application
{
    public static class ApplicationInitializer
    {
        public static void AddDependencyInjections(this IServiceCollection services)
        {
            services.AddScoped<ITodoService, TodoService>();

            // Call Data layer to register its dependencies
            DataInitializer.AddDependencyInjections(services);
        }
    }
}
