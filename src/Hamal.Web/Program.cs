using System.Text;
using Hamal.Application.Common.Interfaces;
using Hamal.Domain.Entities;
using Hamal.Domain.Enums;
using Hamal.Infrastructure.Auth;
using Hamal.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Hamal.Application;

namespace Hamal.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // --- Services Configuration ---

        // JWT Settings
        var jwtSettings = new JwtSettings();
        builder.Configuration.Bind(JwtSettings.SectionName, jwtSettings);
        builder.Services.AddSingleton(Options.Create(jwtSettings));

        // Application services
        builder.Services.AddApplication();

        // Infrastructure Services
        builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

        // Database
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        );

        // Authentication
        builder.Services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
            });

        builder.Services.AddControllers();
        
        // Swagger / OpenAPI
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hamal API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter into field the word 'Bearer' followed by a space and the JWT value",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }});
        });

        // --- Application Build ---
        var app = builder.Build();

        // --- HTTP Request Pipeline Configuration ---
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            // Seed database with admin user for development
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
            if (!dbContext.Users.Any(u => u.Username == "admin"))
            {
                dbContext.Users.Add(new User { Id = Guid.NewGuid(), Username = "admin", PasswordHash = passwordHasher.HashPassword("admin"), Role = Role.Admin });
                dbContext.SaveChanges();
            }
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}