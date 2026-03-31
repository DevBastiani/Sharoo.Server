using Sharoo.Server.API.Hubs;
using Sharoo.Server.API.Middlewares;

namespace Sharoo.Server.API.Extensions
{
    public static class AppExtensions
    {
        public static void UseArchitectures(this WebApplication app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseCors("AllowLocalhost");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sharoo.Server v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapHub<TodoHub>("/hubs/todo");
        }
    }
}
