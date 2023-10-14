using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using XeerLearn.Data;
using XeerLearn.Models.Auth;

namespace XeerLearn.Middleware
{
    #nullable disable
    public class AccessKeyMiddleware     
    {
        private readonly RequestDelegate _next;

        public AccessKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, UserManager<Models.Auth.XeerLearnUser> _manager, ApplicationDbContext _context)
        {
            try
            {
                if (httpContext.User.Identity.IsAuthenticated == true)
                {
                    var user = await _manager.FindByEmailAsync(httpContext.User.Identity.Name);
                    var access = await _context.UserAccess.Where(x => x.XeerLearnUserId == user.Id).FirstOrDefaultAsync();

                    if (access != null)
                    {
                        if (access.ExpiryDate < DateTime.Now)
                        {
                            if (httpContext.Request.Path != "/expired-access")
                                httpContext.Response.Redirect("/expired-access");
                            return;
                        }
                    }
                    
                }
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

            await _next.Invoke(httpContext);

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
