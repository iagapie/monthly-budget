using System;
using System.Text;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Authorization;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using GraphQL.Validation;
using IA.Finance.Api.GraphQL;
using IA.Finance.Api.GraphQL.Authorization;
using IA.Finance.Api.GraphQL.Resolvers.Movement;
using IA.Finance.Api.GraphQL.Resolvers.Project;
using IA.Finance.Api.GraphQL.Resolvers.User;
using IA.Finance.Api.Identity;
using IA.Finance.Api.Jwt;
using IA.Finance.Api.Middlewares;
using IA.Finance.Domain.AggregatesModel.MovementAggregate;
using IA.Finance.Domain.AggregatesModel.ProjectAggregate;
using IA.Finance.Domain.AggregatesModel.UserAggregate;
using IA.Finance.Infrastructure;
using IA.Finance.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IA.Finance.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddHttpContextAccessor();

            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IMovementRepository, MovementRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            services.AddScoped<UsersResolver>();
            services.AddScoped<UserResolver>();
            services.AddScoped<ChangePasswordResolver>();
            services.AddScoped<CreateUserResolver>();
            services.AddScoped<UpdateUserResolver>();
            
            services.AddScoped<ProjectsResolver>();
            services.AddScoped<ProjectResolver>();
            services.AddScoped<CreateProjectResolver>();
            services.AddScoped<UpdateProjectResolver>();
            services.AddScoped<RemoveProjectResolver>();
            
            services.AddScoped<MovementsResolver>();
            services.AddScoped<CreateMovementResolver>();
            services.AddScoped<UpdateMovementResolver>();
            services.AddScoped<RemoveMovementResolver>();
            services.AddScoped<CreateMovementItemResolver>();
            services.AddScoped<UpdateMovementItemResolver>();
            services.AddScoped<RemoveMovementItemResolver>();
            
            services.AddScoped<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));
            services.AddScoped<ISchema, AppSchema>();
            
            services.TryAddSingleton<IAuthorizationEvaluator, AuthorizationEvaluator>();
            services.AddTransient<IValidationRule, AuthorizationValidationRule>();
            
            services.TryAddSingleton(f =>
            {
                var authorization = new AuthorizationSettings();

                authorization.AddPolicy("UserPolicy", _ => _.RequireClaim("rol", "user", "admin"));
                authorization.AddPolicy("AdminPolicy", _ => _.RequireClaim("rol", "admin"));

                return authorization;
            });

            services.AddGraphQL(_ =>
                {
                    _.EnableMetrics = true;
                    _.ExposeExceptions = true;
                })
                .AddGraphTypes(ServiceLifetime.Scoped)
                .AddUserContextBuilder(httpContext => new UserContext {User = httpContext.User})
                .AddDataLoader();
            
            var authSettings = Configuration.GetSection(nameof(AuthSettings));
            services.Configure<AuthSettings>(authSettings);
            
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings[nameof(AuthSettings.SecretKey)]));
            
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                    ValidateAudience = true,
                    ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,

                    RequireExpirationTime = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                
                options.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.SaveToken = true;
                
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddSingleton<ITokenFactory, TokenFactory>();
            services.AddSingleton<IJwtTokenHandler, JwtTokenHandler>();
            services.AddSingleton<IJwtTokenValidator, JwtTokenValidator>();
            services.AddSingleton<IJwtFactory, JwtFactory>();
            
            var identityBuilder = services.AddIdentityCore<IdentityUser>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });
            
            identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(IdentityRole), identityBuilder.Services);
            identityBuilder.AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();

            services.AddDbContext<AppIdentityDbContext>((_, o) =>
            {
                if (!o.IsConfigured)
                {
                    o.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly("IA.Finance.Api"));
                }
            });

            return services.AddEntityFrameworkNpgsql().AddDbContext<FinanceContext>((_, o) =>
            {
                if (!o.IsConfigured)
                {
                    o.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly("IA.Finance.Api"));
                }
            }).BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
                app.UseCors(_ => _.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                
                app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());
            }
            else
            {
                app.UseCors(_ => _.WithOrigins(Configuration["ClientOrigin"].Split(',')).AllowAnyMethod().AllowAnyHeader());
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();

            app.UseAuthentication();
            
            app.UseGraphQL<ISchema>();

            var authSettings = app.ApplicationServices.GetService<IOptions<AuthSettings>>().Value;

            app.UseMiddleware<LoginMiddleware>(new PathString(authSettings.LoginPath));
            app.UseMiddleware<RefreshTokenMiddleware>(new PathString(authSettings.RefreshTokenPath));

            app.Run(async (context) => await context.Response.WriteAsync("Finance API!"));
        }
    }
}