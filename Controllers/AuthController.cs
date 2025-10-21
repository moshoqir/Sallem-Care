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
    [HttpGet("me")]

    public async Task<IActionResult> Me()
    {
        var userId = User.GetUserId();

        var user = await _db.Users.AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new { u.Id, u.Name, u.Email, u.CreatedAt })
            .FirstOrDefaultAsync();

        if (user is null) return NotFound(new { error = new { message = "User not found." } });
        return Ok(user);

    }
}