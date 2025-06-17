using Hamal.Application.Common.Interfaces;
using Hamal.Infrastructure.Auth;
using Hamal.Infrastructure.Background;
using Hamal.Infrastructure.Files;
using Microsoft.Extensions.DependencyInjection;

namespace Hamal.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // File Services
        services.AddScoped<IFileExporter, CsvExporter>();
        services.AddScoped<IFileParser, CsvParser>();
        
        // Auth Services
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        // Background Services
        services.AddHostedService<AbandonedCitizenCleanupJob>();
        
        return services;
    }
} 