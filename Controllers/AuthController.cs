using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleemCare.Api.Data;
using SaleemCare.Api.Domain.Entities;
using BCrypt.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SaleemCare.Api.Extensions;
using System;
using SaleemCare.Api.Dtos.Auth;

namespace SaleemCare.Api.Controllers;

[ApiController]
[Route("v1/auth")]

public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ITokenService _tokens;

    public AuthController(AppDbContext db, ITokenService tokens)
    { _db = db; _tokens = tokens; }

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

        var token = _tokens.Create(user);

        return StatusCode(201, new { token, user = new { user.Id, user.Name, user.Email } });
    }

    [HttpPost("login")]
    public async Task<IActionResult> login([FromBody] LoginDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password,user.PasswordHash))
        { return Unauthorized(new { error = new { message = "Invalid credentials." } });
        }

        var token = _tokens.Create(user);
        return Ok(new { token, user = new { user.Id, user.Name, user.Email } });
    }



    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> changePassword([FromBody] ChangePasswordDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.CurrentPassword) || string.IsNullOrWhiteSpace(dto.NewPassword))
        {
            return BadRequest(new { error = new { message = "Both currentPassword and newPassword are required." } });
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

        var token = _tokens.Create(user); 
        return Ok(new { token });
    }
}