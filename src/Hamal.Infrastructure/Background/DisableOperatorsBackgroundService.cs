using Hamal.Domain.Enums;
using Hamal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hamal.Infrastructure.Background;

public class DisableOperatorsBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public DisableOperatorsBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.Now;
            var nextRun = new DateTime(now.Year, now.Month, now.Day, 23, 0, 0);
            if (now > nextRun)
                nextRun = nextRun.AddDays(1);

            var delay = nextRun - now;
            await Task.Delay(delay, stoppingToken);

            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var operators = await dbContext.Users
                .Where(u => u.Role == Role.Operator && !u.IsDisabled)
                .ToListAsync(stoppingToken);

            foreach (var user in operators)
            {
                user.Disable();
            }

            await dbContext.SaveChangesAsync(stoppingToken);
        }
    }
}