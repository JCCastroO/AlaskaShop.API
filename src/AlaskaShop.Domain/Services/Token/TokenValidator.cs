using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AlaskaShop.Domain.Services.Token;

public class TokenValidator
{
    private readonly string? _key;

    public TokenValidator(string? key) => _key = key;

    public Guid Validate(string token)
    {
        var validParams = new TokenValidationParameters()
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = SecurityKey(),
            ClockSkew = new TimeSpan(0)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, validParams, out _);
        var userIdentifier = principal.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
        return Guid.Parse(userIdentifier);
    }

    private SymmetricSecurityKey SecurityKey()
    {
        var key = Encoding.UTF8.GetBytes(_key ?? "#Error");
        return new SymmetricSecurityKey(key);
    }
}
