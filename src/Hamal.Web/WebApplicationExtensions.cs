using Hamal.Application.Common.Interfaces;
using Hamal.Domain.Entities;
using Hamal.Domain.Enums;
using Hamal.Infrastructure.Persistence;

namespace Hamal.Web;

public static class WebApplicationExtensions
{
    public static async Task ConfigureApplication(this WebApplication app)
    {
        app.UseCors("CorsPolicy");

        // --- HTTP Request Pipeline Configuration ---
        if (app.Environment.IsDevelopment())
        {
            // app.MapOpenApi();

            app.UseSwagger();
            // app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "NavigatorProject"));
            app.UseSwaggerUI();
        }

        await app.SeedAdmin();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }

    private static async Task SeedAdmin(this WebApplication app)
    {
        // Seed database with admin user for development
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        
        var userName = Environment.GetEnvironmentVariable("ADMINUSER_NAME");
        var password = Environment.GetEnvironmentVariable("ADMINUSER_PASSWORD");
        if(string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            throw new NullReferenceException("Admin username and password are required");
        
        if (!dbContext.Users.Any(u => u.Username == userName))
        {
            // Create an admin user if it does not exist
            var user = User.Create(userName, passwordHasher.HashPassword(password), Role.Admin);
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }
    }
}