using System.Security.Claims;

namespace IA.Finance.Api.Jwt
{
    public interface IJwtTokenValidator
    {
        ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey);
    }
}