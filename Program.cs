
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SaleemCare.Api.Data;
using SaleemCare.Api.Data.Seed;
using FluentValidation.AspNetCore;
using SaleemCare.Api.Services;
using SaleemCare.Api.Services.Excel;
using SaleemCare.Api.Middleware;
using SaleemCare.Api.Services.Background;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using SaleemCare.Api.Domain.Entities;
using System.Security.Claims;





var builder = WebApplication.CreateBuilder(args);

// EF Core (SQL Server)

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));






// Controllers & Swagger


builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITokenService, TokenService>();

// AI servicse
builder.Services.AddHttpClient();
builder.Services.AddSingleton<GoogleAiService>();

// Excel Service
builder.Services.AddScoped<ExcelImportService>();

// Background service
builder.Services.AddHostedService<GuestCleanupService>();

// voice transcription service
builder.Services.AddScoped<IVoiceTranscriptionService, FakeVoiceTranscriptionService>();

var jwt = builder.Configuration.GetSection("Jwt");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!))
        };

    });


// CORS (Flutter)

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("flutter", p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
});



// rate limiter for guest users
builder.Services.AddRateLimiter(options =>
{
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";

        await context.HttpContext.Response.WriteAsync(
            "{\"error\":\"Too many guest login attempts. Please wait and try again.\"}", token
            );
    };

    static string GetUserKey(HttpContext httpContext)
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? httpContext.User.FindFirstValue("sub");

        if (!string.IsNullOrEmpty(userId))
        {
            return $"user: {userId}";
        }

        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return $"ip : {ip}";
    }


    // for guest
    options.AddPolicy("GuestAuthPolicy", HttpContext =>
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 5,
            Window = TimeSpan.FromMinutes(1),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0
        });
    });



    // for auth login
    options.AddPolicy("AuthLoginPolicy", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromMinutes(5),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0
        });
    });

    // for register
    options.AddPolicy("AuthRegisterPolicy", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 3,
            Window = TimeSpan.FromHours(1),
            QueueProcessingOrder =  QueueProcessingOrder.OldestFirst,
            QueueLimit = 0
        });
    });

    // for change password
    options.AddPolicy("AuthChangePasswordPolicy", httpContext =>
    {
        var key = GetUserKey(httpContext);

        return RateLimitPartition.GetFixedWindowLimiter(key, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 3, 
            Window = TimeSpan.FromMinutes(10),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0
        });
    });

    // for AI chatbot
    options.AddPolicy("ChatbotPolicy", httpContext =>
    {
        var key = GetUserKey(httpContext);

        return RateLimitPartition.GetFixedWindowLimiter(key, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 20,
            Window = TimeSpan.FromMinutes(1),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0
        });
    });

    // AI diagnosis
    options.AddPolicy("DiagnosisPolicy", httpContext =>
    {
        var key = GetUserKey(httpContext);

        return RateLimitPartition.GetFixedWindowLimiter(key, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 30,
            Window = TimeSpan.FromHours(1),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0
        });
    });

    // Admin : Excel managament
    options.AddPolicy("AdminExcelPolicy", httpContext =>
    {
        var key = GetUserKey(httpContext);

        return RateLimitPartition.GetFixedWindowLimiter(key, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 5,
            Window = TimeSpan.FromMinutes(10),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0
        });

    });


    // for AI Voice
    options.AddPolicy("VoicePolicy", httpContext =>
    {
        var key = GetUserKey(httpContext);

        

        return RateLimitPartition.GetFixedWindowLimiter(key, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromMinutes(30),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0
        });
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Auto-migrate + seed on startup (dev-safe)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    await DbInitializer.SeedAsync(db);
}

app.UseCors("flutter");
app.UseAuthentication();
app.UseMiddleware<GuestExpirationMiddleware>();
app.UseRateLimiter();
app.UseAuthorization();

app.MapControllers();
app.Run();