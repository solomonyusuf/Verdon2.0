using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using NToastNotify;
using System.Data;
using XeerLearn.Controllers;
using XeerLearn.Data;
using XeerLearn.Models.Auth;
using XeerLearn.Models.Utility;

namespace XeerLearn.Pages.Components.Tutor
{
    [Authorize(Roles = "Tutor")]
    public class TutorLecturesModel : PageModel
    {
        [BindProperty]
        public Meeting meeting { get; set; }
        [BindProperty]
        public UserAccess access { get; set; } 
        [BindProperty]
        public List<Meeting> meetings { get; set; }
        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;
        public readonly StreamController _stream;
        public readonly IConfiguration _config;
        public readonly UserManager<Models.Auth.XeerLearnUser> _manager;
        public readonly IHttpContextAccessor _acc;
        public TutorLecturesModel(IHttpContextAccessor acc ,UserManager<Models.Auth.XeerLearnUser> manager, IConfiguration config,StreamController stream,IToastNotification toast,ApplicationDbContext context)
        {
            _acc = acc;
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
                access = await _context.UserAccess.Where(x => x.AccessKeyId == Guid.Parse(_acc.HttpContext.Session.GetString("AccessKey"))).FirstAsync();
                meetings = await _context.Meetings.Where(x=> x.XeerLearnUserId == user.Id).OrderByDescending(x=> x.Id).ToListAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public async Task<IActionResult> OnPost()
        {
            try
            {
               if(meeting.StartTime < DateTime.Now || meeting.StartTime == DateTime.Now)
                {
                    _toast.AddErrorToastMessage("The starting time must be greater than the current time");
                    return Redirect("/tutor-lectures");
                }

                meeting.AccessKeyId = access.AccessKeyId;
                await _context.Meetings.AddAsync(meeting);
                await _context.SaveChangesAsync();
                _stream.CreateMeeting(meeting);
                _toast.AddSuccessToastMessage("New Lecture Created");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                _toast.AddErrorToastMessage("An error occured");
               
            }

            return Redirect("/tutor-lectures");
        }

        

    }
}
