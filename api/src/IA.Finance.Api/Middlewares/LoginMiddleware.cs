using System.Net;
using System.Threading.Tasks;
using IA.Finance.Api.Jwt;
using IA.Finance.Domain.AggregatesModel.UserAggregate;
using Microsoft.AspNetCore.Http;

namespace IA.Finance.Api.Middlewares
{
    public class LoginMiddleware : AMiddleware
    {
        public LoginMiddleware(RequestDelegate next, PathString path) : base(next, path)
        {
        }

        protected override async Task Handle(HttpContext context)
        {
            if (context.Request.IsJsonPost())
            {
                try
                {
                    var loginModel = context.Request.Body.Deserialize<LoginModel>();

                    var userRepository = context.Get<IUserRepository>();
                    var jwtFactory = context.Get<IJwtFactory>();
                    
                    var user = await userRepository.FindByUserName(loginModel.UserName);

                    if (user != null && await userRepository.CheckPassword(user, loginModel.Password))
                    {
                        var tokenFactory = context.Get<ITokenFactory>();

                        var refreshToken = tokenFactory.GenerateToken();
                        
                        user.AddRefreshToken(refreshToken, context.Connection.RemoteIpAddress?.ToString());

                        await userRepository.Update(user);

                        var accessToken = await jwtFactory.GenerateEncodedToken(user);

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
                catch
                {
                    // ignored
                }
            }
            
            await context.JsonResponse(new
            {
                error = new
                {
                    code = (int) HttpStatusCode.Unauthorized,
                    message = "Invalid Username or Password."
                }
            }, HttpStatusCode.Unauthorized);
        }

        private class LoginModel
        {
            public string UserName { get; set; }
            
            public string Password { get; set; }
        }
    }
}