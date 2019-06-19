using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using IA.Finance.Api.Identity;
using IA.Finance.Api.Jwt;
using IA.Finance.Domain.AggregatesModel.UserAggregate;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace IA.Finance.Api.Middlewares
{
    public class RefreshTokenMiddleware : AMiddleware
    {
        public RefreshTokenMiddleware(RequestDelegate next, PathString path) : base(next, path)
        {
        }

        protected override async Task Handle(HttpContext context)
        {
            if (context.Request.IsJsonPost())
            {
                try
                {
                    var headers = context.Request.Headers.Where(x => new[] {"authorization", "refresh-token"}.Contains(x.Key.ToLowerInvariant())).ToList();
                    
                    string oldAccessToken = headers.Where(x => x.Key.ToLowerInvariant() == "authorization").Select(x => x.Value).FirstOrDefault();
                    string oldRefreshToken = headers.Where(x => x.Key.ToLowerInvariant() == "refresh-token").Select(x => x.Value).FirstOrDefault();

                    if (oldAccessToken != null && oldRefreshToken != null)
                    {
                        if (oldAccessToken.ToLowerInvariant().StartsWith("bearer "))
                        {
                            oldAccessToken = oldAccessToken.Substring("bearer ".Length);
                        }

                        var jwtTokenValidator = context.Get<IJwtTokenValidator>();
                        var authSettings = context.Get<IOptions<AuthSettings>>().Value;

                        var principal = jwtTokenValidator.GetPrincipalFromToken(oldAccessToken, authSettings.SecretKey);

                        var id = (principal?.Identity as ClaimsIdentity)?.Claims.First(c => c.Type == "id");

                        if (id != null)
                        {
                            var userRepository = context.Get<IUserRepository>();
                            var user = await userRepository.FindByIdentityId(id.Value);

                            if (user != null && user.HasValidRefreshToken(oldRefreshToken))
                            {
                                var jwtFactory = context.Get<IJwtFactory>();
                                var tokenFactory = context.Get<ITokenFactory>();

                                var accessToken = await jwtFactory.GenerateEncodedToken(user);
                                var refreshToken = tokenFactory.GenerateToken();

                                user.RemoveRefreshToken(oldRefreshToken);
                                user.AddRefreshToken(refreshToken, context.Connection.RemoteIpAddress?.ToString());

                                await userRepository.Update(user);

                                await context.JsonResponse(new
                                {
                                    data = new
                                    {
                                        token = new
                                        {
                                            access = accessToken,
                                            refresh = refreshToken
                                        }
                                    }
                                });
                                return;
                            }
                        }
                    }
                }
                catch
                {
                    // ignored
                }
            }

            await context.JsonResponse(new
            {
                error = new
                {
                    code = (int) HttpStatusCode.BadRequest,
                    message = "Invalid token."
                }
            }, HttpStatusCode.BadRequest);
        }
    }
}