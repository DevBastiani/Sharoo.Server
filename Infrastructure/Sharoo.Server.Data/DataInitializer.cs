using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sharoo.Server.Data.Repositories.Todos;
using Sharoo.Server.Data.Repositories.Users;
using Sharoo.Server.Domain.Entities;
using Sharoo.Server.Domain.Utilities;

namespace Sharoo.Server.Data
{
    public static class DataInitializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITodoRepository, TodoRepository>();
        }

        public static void InitializeDatabase(this IServiceCollection services)
        {
            services.AddDbContext<SharooDbContext>(options =>
                options.UseInMemoryDatabase("SharooDb"));
        }

        public static void SeedDatabase(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SharooDbContext>();

                context.Database.EnsureCreated();

                if (context.Users.Any()) return;

                var userId = Guid.NewGuid();
                var hashedPassword = PasswordHasher.HashPassword("Senha@123");

                var testUser = new User
                {
                    Id = userId,
                    Name = "João Silva",
                    Email = "joao@example.com",
                    Password = hashedPassword,
                    Role = new List<string> { "COMMON" },
                    CreatedAt = DateTime.UtcNow
                };

                context.Users.Add(testUser);
                context.SaveChanges();

                var now = DateTime.UtcNow;
                context.Todos.AddRange(
                    new Todo
                    {
                        Id = Guid.NewGuid(),
                        Name = "Estudar C#",
                        Description = "Aprender conceitos avançados de C#",
                        UserId = userId,
                        IsDone = false,
                        CreatedAt = now,
                        CompletedAt = null
                    },
                    new Todo
                    {
                        Id = Guid.NewGuid(),
                        Name = "Implementar API",
                        Description = "Criar API REST com .NET 8",
                        UserId = userId,
                        IsDone = true,
                        CreatedAt = now.AddDays(-5),
                        CompletedAt = now
                    },
                    new Todo
                    {
                        Id = Guid.NewGuid(),
                        Name = "Configurar Database",
                        Description = "Configurar Entity Framework Core",
                        UserId = userId,
                        IsDone = false,
                        CreatedAt = now.AddDays(-2),
                        CompletedAt = null
                    }
                );

                context.SaveChanges();
            }
        }
    }
}
