using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace IA.Finance.Api.Jwt
{
    public interface IJwtTokenHandler
    {
        string WriteToken(JwtSecurityToken jwt);
        
        ClaimsPrincipal ValidateToken(string token, TokenValidationParameters tokenValidationParameters);
    }
}