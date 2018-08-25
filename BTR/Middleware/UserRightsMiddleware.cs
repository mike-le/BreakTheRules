using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace BTR.Middleware
{
    public class UserRightsMiddleware
    {
        private readonly RequestDelegate _next;

        public UserRightsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, UNOSUserPrincipalService userPrincipalService)
        {
            if (userPrincipalService.IsBlackListed)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.Headers.Add("IsBlacklisted", "true");
                return;
            }

            await _next(context);
        }
    }
}
