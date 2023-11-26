using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UniverseRepo.Application.DTOs.Response;
using UniverseRepo.Application.Models;

namespace UniverseRepo.Core.Services;

public interface ITokenService
{
    Task<LoginResponseDto> BuildJwtSecurityToken(User user);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}

public class TokenService : ITokenService
{
    private readonly Token _tokenConfig;

    public TokenService(Token tokenConfig)
    {
        _tokenConfig = tokenConfig;
    }

    public async Task<LoginResponseDto> BuildJwtSecurityToken(User user)
    {
        var bytes = Encoding.UTF8.GetBytes(_tokenConfig.Secret);
        var key = new SymmetricSecurityKey(bytes);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.Sid, user.Id.ToString())
        };

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _tokenConfig.Issuer,
            audience: _tokenConfig.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_tokenConfig.AccessTokenExpiration),
            signingCredentials: credentials);

        //user.RefreshToken = BuildRefreshToken();
        //user.RefreshTokenExpiration = DateTime.Now.AddDays(_tokenConfig.RefreshTokenExpiration);
        //await _context.SaveChangesAsync();

        return new LoginResponseDto
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = user.RefreshToken,
            ExpirationDate = token.ValidTo.ToLocalTime()
        };
    }
    
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.Secret)),
            ValidateLifetime = false
        };

        var principal = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out var securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture))
        {
            throw new Exception("Invalid token.");
        }

        return principal;
    }
}