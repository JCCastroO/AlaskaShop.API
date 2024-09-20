using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AlaskaShop.Domain.Services.Token;

public class JwtTokenGenerator
{
    private readonly int _expiration;
    private readonly string? _key;

    public JwtTokenGenerator(string? key, int expiration)
    {
        _key = key;
        _expiration = expiration;
    }

    public string Generate(Guid userIdentifier)
    {
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Expires = DateTime.UtcNow.AddSeconds(_expiration),
            SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature),
            Subject = new ClaimsIdentity([new Claim(ClaimTypes.Sid, userIdentifier.ToString())])
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private SymmetricSecurityKey SecurityKey()
    {
        var key = Encoding.UTF8.GetBytes(_key);
        return new SymmetricSecurityKey(key);
    }
}
