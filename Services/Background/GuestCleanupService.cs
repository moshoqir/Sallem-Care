using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SaleemCare.Api.Data;

namespace SaleemCare.Api.Services.Background;

public class GuestCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    private readonly IConfiguration _config;

    public GuestCleanupService(IServiceScopeFactory scopeFactory, IConfiguration config)
    {
        _scopeFactory = scopeFactory;
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CleanupExpiredGuests(stoppingToken);
            }
            catch { }

            var intervalMinutes = _config.GetValue<int?>("Guests:CleanUpIntervalMinutes") ?? 60;
            await Task.Delay(TimeSpan.FromMinutes(intervalMinutes), stoppingToken);
        }
    }

    private async Task CleanupExpiredGuests(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var now = DateTime.UtcNow;

        var expiredGuests = await db.Users
            .Where(u => u.IsGuest && u.GuestExpiresAt.HasValue && u.GuestExpiresAt < now)
            .Select(u => u.Id)
            .ToListAsync(ct);

        if (!expiredGuests.Any())
        {
            return;
        }

        // reomve related data of this guest


        // answers
        var answers = await db.UserSymptomAnswers
            .Where(a => expiredGuests.Contains(a.UserId))
            .ToListAsync(ct);
        db.UserSymptomAnswers.RemoveRange(answers);

        // encounters (sessions)
        var encounters = await db.Encounters
            .Where(e => expiredGuests.Contains(e.UserId))
            .ToListAsync(ct);
        db.Encounters.RemoveRange(encounters);


        // profile
        var profile = await db.UserProfiles
            .Where(p => expiredGuests.Contains(p.UserId))
            .ToListAsync(ct);
        db.UserProfiles.RemoveRange(profile);

        // the user
        var users = await db.Users
            .Where(u => expiredGuests.Contains(u.Id))
            .ToListAsync(ct);
        db.Users.RemoveRange(users);

        await db.SaveChangesAsync();

    }
}


