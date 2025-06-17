using Hamal.Domain.Enums;
using Hamal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hamal.Infrastructure.Background;

/// <summary>
/// A background service that periodically scans for and cleans up abandoned citizen records.
/// An abandoned record is one that is 'InProgress' but its lock has expired.
/// </summary>
public class AbandonedCitizenCleanupJob(
    IServiceScopeFactory scopeFactory,
    ILogger<AbandonedCitizenCleanupJob> logger) : BackgroundService
{
    // The task suggests every 5-10 minutes. We'll use 5 minutes.
    private readonly TimeSpan _period = TimeSpan.FromMinutes(5);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(_period);
        
        // Wait for the first tick to avoid running immediately at startup
        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            await DoWorkAsync(stoppingToken);
        }
    }

    private async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Abandoned citizen cleanup job is running.");

        try
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var updatedCount = await dbContext.Citizens
                .Where(c => c.Status == CitizenStatus.InProgress && c.LockedUntil < DateTime.UtcNow)
                .ExecuteUpdateAsync(updates => updates
                    .SetProperty(c => c.Status, CitizenStatus.Pending)
                    .SetProperty(c => c.AssignedToUserId, (Guid?)null)
                    .SetProperty(c => c.LockedUntil, (DateTime?)null),
                    cancellationToken: stoppingToken);
            
            if (updatedCount > 0)
            {
                logger.LogInformation("Cleaned up {Count} abandoned citizen records.", updatedCount);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while cleaning up abandoned citizen records.");
        }
    }
} 