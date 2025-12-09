using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SaleemCare.Api.Data;

namespace SaleemCare.Api.Middleware;

public class GuestExpirationMiddleware
{
    private readonly RequestDelegate _next;

    public GuestExpirationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context,AppDbContext db)
    {
        if (context.User?.Identity?.IsAuthenticated != true)
        {
            await _next(context);
            return;
        }

        // check isGuest claim
        var isGuest = context.User.Claims
            .FirstOrDefault(c => c.Type == "IsGuest")?.Value == "true";

        if (!isGuest)
        {
            await _next(context);
            return;
        }

        // get user id form token 
        var sub = context.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? context.User.FindFirstValue("sub");

        if (!Guid.TryParse(sub, out var userId)) 
        { 
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { error = "Invalid user token." });
            return;
        }

        var user = await db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u=> u.Id == userId);

        if (user is null || !user.IsGuest)
        {
            await _next(context);
            return;
        }

        // if expired -> block

        if (user.GuestExpiresAt.HasValue && user.GuestExpiresAt.Value < DateTime.UtcNow)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Guest session expired. Please start again or create an account."
            });
            return;
        }
        await _next(context);
    }
}
