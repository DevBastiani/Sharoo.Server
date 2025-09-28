using Microsoft.Extensions.DependencyInjection;
using Sharoo.Server.Application.Services.Todos;
using Sharoo.Server.Application.Services.Users;
using Sharoo.Server.Data;

namespace Sharoo.Server.Application
{
    public static class ApplicationInitializer
    {
        public static void AddDependencyInjections(this IServiceCollection services)
        {
            InitializeServices(services);

            DataInitializer.InitializeRepositories(services);
            DataInitializer.InitializeDatabase(services);
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITodoService, TodoService>();
        }
    }
}
