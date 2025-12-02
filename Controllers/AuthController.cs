using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleemCare.Api.Data;
using SaleemCare.Api.Dtos.Auth;
using SaleemCare.Api.Domain.Entities;
using BCrypt.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SaleemCare.Api.Extensions;
using Microsoft.Extensions.Configuration;
using System;

namespace SaleemCare.Api.Controllers;

[ApiController]
[Route("v1/auth")]

public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ITokenService _tokens;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext db, ITokenService tokens, IConfiguration config)
    { _db = db; _tokens = tokens; _config = config; }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
        {
            return BadRequest(new { error = new { message = "Email and Password are required." } });
        }

        var exists = await _db.Users.AnyAsync(u => u.Email == dto.Email);
        if (exists) return Conflict(new { error = new { message = "Email already exists." } });

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

   

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var patientRole = await _db.Roles.FirstAsync(r => r.Name == "Patient");
        _db.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = patientRole.Id
        });
        await _db.SaveChangesAsync();

        var roles = await GetRoleNamesAsync(user.Id);
        var token = _tokens.Create(user, roles);

        return StatusCode(201, new { token, user = new { user.Id, user.Name, user.Email } });
    }

    [HttpPost("login")]
    public async Task<IActionResult> login([FromBody] LoginDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password,user.PasswordHash))
        { return Unauthorized(new { error = new { message = "Invalid credentials." } });
        }

        var roles = await GetRoleNamesAsync(user.Id);
        var token = _tokens.Create(user, roles);

       
        return Ok(new { token, user = new { user.Id, user.Name, user.Email } });
    }


    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> changePassword([FromBody] ChangePasswordDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.CurrentPassword) || string.IsNullOrWhiteSpace(dto.NewPassword))
        {
            return BadRequest(new { error = new { message = "Both currentPassword and new Password are required." } });
        }

        if (dto.NewPassword.Length < 8)
            return BadRequest(new { error = new { message = "New password must be at least 8 characters." } });



        var userId = User.GetUserId();

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null) return Unauthorized();

        var ok = BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash);

        if (!ok) return Unauthorized(new { error = new { message = "Current password is incorrect." } });

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

        await _db.SaveChangesAsync();

        return Ok(new { message = "Password changed successfully." });


    }

    [Authorize]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var userId = User.GetUserId();

        var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null) return Unauthorized();

        var roles = await GetRoleNamesAsync(user.Id);
        var token = _tokens.Create(user, roles);
        return Ok(new { token });
    }


    private async Task<List<string>> GetRoleNamesAsync(Guid userId)
    {
        return await _db.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role!.Name)
            .ToListAsync();
    }

    [HttpPost("guest")]
    [AllowAnonymous]
    public async Task<IActionResult> Guest([FromBody]  GuestLoginRequest? dto)
    {

        // read time to delete the gues after the give time
        var now = DateTime.UtcNow;

        var lifetime = _config.GetValue<int?>("Guests:LifetimeHours") ?? 48;

        // create fake email for this guest (email can't be null)
        var pseudoEmail = $"guest_{Guid.NewGuid():N}@guest.local";

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = pseudoEmail,
            Name = string.IsNullOrWhiteSpace(dto?.DisplayName) ? "Guest" : dto.DisplayName.Trim(),
            IsGuest = true,
            PasswordHash = string.Empty, 
            CreatedAt = now,
            GuestExpiresAt = now.AddHours(lifetime),
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var patientRole = await _db.Roles.FirstAsync(r => r.Name == "Patient");
        _db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = patientRole.Id});

        await _db.SaveChangesAsync();

        // Issue token
        var roles = await GetRoleNamesAsync(user.Id);
        var token = _tokens.Create(user,roles);

        return Ok(new
        {
            token,
            user = new
            {
                user.Id,
                user.Name,
                user.Email,
                user.IsGuest,
                
            }
        });
    }



}