using System;
using System.Security.Cryptography;

namespace IA.Finance.Api.Jwt
{
    public class TokenFactory : ITokenFactory
    {
        public string GenerateToken(int size = 32)
        {
            var data = new byte[size];
            
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(data);
                return Convert.ToBase64String(data);
            }
        }
    }
}