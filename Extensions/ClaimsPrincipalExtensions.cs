using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System;

namespace SaleemCare.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var sub = user.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(sub)) { throw new InvalidOperationException("No user id claim in token."); }

        return Guid.Parse(sub);
    }

}