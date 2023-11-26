using System.Security.Claims;
using UniverseRepo.Core.Services;

namespace UniverseRepo.Core.Helpers;

public interface ITokenHelper
{
    string GetRequesterId(string token);
}

public class TokenHelper : ITokenHelper
{
    private readonly ITokenService _tokenService;

    public TokenHelper(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public string GetRequesterId(string token)
    {
        var tokenToUse = token.Split(" ")[1];
        var principal = _tokenService.GetPrincipalFromExpiredToken(tokenToUse);
        
        var userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
        return userId ?? throw new InvalidOperationException();
    }
}