using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NToastNotify;
using System.Data;
using XeerLearn.Controllers;
using XeerLearn.Data;
using XeerLearn.Models.Auth;
using XeerLearn.Models.Utility;

namespace XeerLearn.Pages.Components.Student
{
    [Authorize(Roles = "Student")]
    public class StudentLecturesModel : PageModel
    {
        [BindProperty]
        public Meeting meeting { get; set; } 
        public Guid Access { get; set; } 
        public List<Meeting> meetings { get; set; }
        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;
        public readonly StreamController _stream;
        public readonly IConfiguration _config;
        public readonly UserManager<Models.Auth.XeerLearnUser> _manager;

        public StudentLecturesModel(UserManager<Models.Auth.XeerLearnUser> manager, IConfiguration config,StreamController stream,IToastNotification toast,ApplicationDbContext context)
        {
            _manager = manager;
            _config = config;
            _stream = stream;
            _toast = toast;
            _context = context;
           
        }
        public async Task OnGet()
        {
            try
            {
                var user = await _manager.FindByEmailAsync(User.Identity.Name);
                var access = await _context.UserAccess.Where(x => x.XeerLearnUserId == user.Id).FirstAsync();
                meetings = await _context.Meetings.Where(x=> x.AccessKeyId == access.AccessKeyId).OrderByDescending(x=> x.Id).ToListAsync();
                
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
   


       

    }
}
