using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NToastNotify;
using System.Data;
using Verdon.Controllers;
using Verdon.Data;
using Verdon.Models.Auth;
using Verdon.Models.Utility;

namespace Verdon.Pages.Components.Tutor
{
    [Authorize(Roles = "Tutor")]
    public class TutorLecturesModel : PageModel
    {
        [BindProperty]
        public Meeting meeting { get; set; } 
        public Guid Access { get; set; } 
        public List<Meeting> meetings { get; set; }
        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;
        public readonly StreamController _stream;
        public readonly IConfiguration _config;
        public readonly UserManager<VerdonUser> _manager;

        public TutorLecturesModel(UserManager<VerdonUser> manager, IConfiguration config,StreamController stream,IToastNotification toast,ApplicationDbContext context)
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
                var access = await _context.UserAccess.Where(x => x.VerdonUserId == user.Id).FirstAsync();
                meetings = await _context.Meetings.Where(x=> x.VerdonUserId == user.Id).OrderByDescending(x=> x.Id).ToListAsync();
                meeting.AccessKeyId = access.AccessKeyId;
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
