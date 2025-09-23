using Sharoo.Server.Application;

namespace Sharoo.Server.API.Extensions
{
    public static class BuilderExtensions
    {
        public static void AddArchitectures(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }

        public static void AddModules(this IServiceCollection services)
        {
            ApplicationInitializer.AddDependencyInjections(services);
        }
    }
}
