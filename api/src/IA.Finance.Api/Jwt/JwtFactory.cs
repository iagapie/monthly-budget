using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using IA.Finance.Domain.AggregatesModel.UserAggregate;
using Microsoft.Extensions.Options;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace IA.Finance.Api.Jwt
{
    public class JwtFactory : IJwtFactory
    {
        private readonly IJwtTokenHandler _jwtTokenHandler;

        private readonly JwtIssuerOptions _jwtIssuerOptions;

        public JwtFactory(IJwtTokenHandler jwtTokenHandler, IOptions<JwtIssuerOptions> jwtIssuerOptions)
        {
            if (jwtIssuerOptions == null)
            {
                throw new ArgumentNullException(nameof(jwtIssuerOptions));
            }

            _jwtTokenHandler = jwtTokenHandler ?? throw new ArgumentNullException(nameof(jwtTokenHandler));
            _jwtIssuerOptions = jwtIssuerOptions.Value;
            
            if (_jwtIssuerOptions.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }
            
            if (_jwtIssuerOptions.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (_jwtIssuerOptions.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }

        public async Task<string> GenerateEncodedToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtIssuerOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtIssuerOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                new Claim("rol", user.Role),
                new Claim("id", user.IdentityId)
            };
            
            var jwt = new JwtSecurityToken(
                _jwtIssuerOptions.Issuer,
                _jwtIssuerOptions.Audience,
                claims,
                _jwtIssuerOptions.NotBefore,
                _jwtIssuerOptions.Expiration,
                _jwtIssuerOptions.SigningCredentials);

            return _jwtTokenHandler.WriteToken(jwt);
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date) =>
            (long) Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .TotalSeconds);
    }
}