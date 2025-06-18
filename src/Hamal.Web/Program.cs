using System.Text;
using Hamal.Application.Common.Interfaces;
using Hamal.Domain.Entities;
using Hamal.Domain.Enums;
using Hamal.Infrastructure;
using Hamal.Infrastructure.Auth;
using Hamal.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Hamal.Application;
using Microsoft.OpenApi.Models;

namespace Hamal.Web;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.UseUrls("http://localhost:5051");
        
        var configuration = builder.Configuration;
        var services = builder.Services;

        // --- Services Configuration ---
        services.AddProgramDependencies(configuration);

        // --- Application Build ---
        var app = builder.Build();

        await app.ConfigureApplication();
        await app.RunAsync();
    }
}