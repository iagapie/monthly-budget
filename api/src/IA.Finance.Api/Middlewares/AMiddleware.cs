using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace IA.Finance.Api.Middlewares
{
    public abstract class AMiddleware
    {
        protected readonly RequestDelegate next;
        protected readonly PathString path;
        
        public AMiddleware(RequestDelegate next, PathString path)
        {
            this.next = next;
            this.path = path;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments(path))
            {
                await Handle(context);
            }
            else
            {
                await next(context);
            }
        }

        protected abstract Task Handle(HttpContext context);
    }
}