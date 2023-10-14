using ChatGPT.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Data;
using XeerLearn.Data;
using XeerLearn.Models.Auth;

namespace XeerLearn.Pages.Components.Tutor
{
    [Authorize(Roles = "Tutor")]
    [DisableRateLimiting]
    public class AISearchModel : PageModel
    {
        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;
        public readonly IConfiguration _config;
        public dynamic response { get; set; }
        public ChatGpt bot { get; set; } 
        public AISearchModel(IConfiguration config,ApplicationDbContext context, IToastNotification toast)
        {
            _config = config;
            _context = context;
            _toast = toast;
            bot = new ChatGpt(_config.GetConnectionString("gpt_key"));
        }     
        public async Task OnGetAsync(string value)
        {
            try
            {
                

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
         
    }
}
