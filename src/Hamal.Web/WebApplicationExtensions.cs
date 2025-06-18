using Hamal.Application.Common.Interfaces;
using Hamal.Domain.Entities;
using Hamal.Domain.Enums;
using Hamal.Infrastructure.Persistence;

namespace Hamal.Web;

public static class WebApplicationExtensions
{
    public static async  Task ConfigureApplication(this WebApplication app)
    {
        app.UseCors("CorsPolicy");

        // --- HTTP Request Pipeline Configuration ---
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            app.UseSwagger();
            // app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "NavigatorProject"));
            app.UseSwaggerUI();

            // Seed database with admin user for development
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
            if (!dbContext.Users.Any(u => u.Username == "admin"))
            {
                dbContext.Users.Add(new User
                {
                    Id = Guid.NewGuid(), Username = "admin", PasswordHash = passwordHasher.HashPassword("admin"),
                    Role = Role.Admin
                });
                dbContext.SaveChanges();
            }
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }
}