using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Ernaehrbar.Tests.Integration.TestHelpers;

/// <summary>
/// Utility for creating Supabase JWT tokens for testing.
/// </summary>
public class SupabaseJwtUtility
{
    private readonly SecurityKey _issuerSigningKey;
    private readonly string _algorithm;
    private readonly SigningCredentials _signingCredentials;

    public SupabaseJwtUtility(string supabaseIssuer, string supabaseJwtSecret)
    {
        Issuer = supabaseIssuer;
        _issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(supabaseJwtSecret));
        _algorithm = SecurityAlgorithms.HmacSha256;
        _signingCredentials = new SigningCredentials(_issuerSigningKey, _algorithm);
    }

    public string Issuer { get; }

    public string ToJwtTokenString(SupabaseJwtContent content)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, content.UserId.ToString()),
            new Claim(ClaimTypes.Email, content.Email),
            new Claim("sub", content.UserId.ToString()),
            new Claim("app_metadata", System.Text.Json.JsonSerializer.Serialize(content.AppMetadata)),
            new Claim("user_metadata", System.Text.Json.JsonSerializer.Serialize(content.UserMetadata))
        };

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: "authenticated",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: _signingCredentials
        );

        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(token);
    }
}
