using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Verdon.Data;
using Verdon.Models.Auth;

namespace Verdon.Middleware
{
    #nullable disable
    public class AccessKeyMiddleware     
    {
        private readonly RequestDelegate _next;

        public AccessKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, UserManager<VerdonUser> _manager, ApplicationDbContext _context)
        {
            if(httpContext.User.Identity.IsAuthenticated == false)  await _next.Invoke(httpContext);
            else
            {
                var user = await _manager.FindByEmailAsync(httpContext.User.Identity.Name);
                var access = await _context.UserAccess.Where(x => x.VerdonUserId == user.Id).FirstOrDefaultAsync();

                if (access.ExpiryDate > DateTime.Now)
                    await _next.Invoke(httpContext);
                else
                    httpContext.Response.Redirect("/expired-access");
            }
           
        }
        
    }
    public static class AccessKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseAccessKeyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AccessKeyMiddleware>();
        }
    }
}
