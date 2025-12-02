using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SaleemCare.Api.Domain.Entities;
using System;

public class TokenService : ITokenService
{
    private readonly IConfiguration _cfg;
    public TokenService(IConfiguration cfg) => _cfg = cfg;

    public string Create(User user, IEnumerable<string> roles , TimeSpan? lifetime = null)
    {
        //read from appsettings confg
        var jwt = _cfg.GetSection("Jwt");
        // read the key and convirt it to UTF8
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
        // creating the crediantials to ensure token is sign
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.Add(lifetime ?? TimeSpan.FromDays(7));


        // Claim is to help APIs to understand who the user is by their info (userId, Email, Name, etc.)
        var claims = new List<Claim>
        {
            // claim to represent userId
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            // claim to represent Email
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("name", user.Name),

            // user guest
            new Claim("isGuest", user.IsGuest ? "true" : "false"),
        };

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));


        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds);


        return new JwtSecurityTokenHandler().WriteToken(token);


    }
}