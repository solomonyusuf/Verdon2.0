using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using XeerLearn.Controllers;
using XeerLearn.Data;
using XeerLearn.Models.Auth;
using XeerLearn.Models.Utility;

namespace XeerLearn.Pages.Components.Lecture
{
    [Authorize(Roles ="Tutor")]
    public class AnnouncementModel : PageModel
    {
        [BindProperty]
        public Announcement alert { get; set; }
        public Models.Auth.XeerLearnUser user { get; set; }
        [BindProperty]
        public Guid Access { get; set; }
        public List<Announcement> alerts { get; set; }
        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;
        public readonly StreamController _stream;
        public readonly IConfiguration _config;
        public readonly UserManager<Models.Auth.XeerLearnUser> _manager;
        public readonly IHttpContextAccessor _acc;

        public AnnouncementModel(IHttpContextAccessor acc,UserManager<Models.Auth.XeerLearnUser> manager, IConfiguration config, StreamController stream, IToastNotification toast, ApplicationDbContext context)
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
                user = await _manager.FindByEmailAsync(User.Identity.Name);
                alerts = await _context.Announcements.Where(x => x.XeerLearnUserId == user.Id).OrderByDescending(x => x.Id).ToListAsync();
                var access = await _context.UserAccess.Where(x => x.AccessKeyId == (Guid.Parse(_acc.HttpContext.Session.GetString("AccessKey")))).FirstAsync();
                Access = access.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public async Task<IActionResult> OnPost()
        {
            try
            {
                await _context.Announcements.AddAsync(alert);
                await _context.JobQueue.AddAsync(new JobQueue
                {
                    Type = "tutor",
                    AccessKeyId = alert.AccessKeyId,
                    ConstrainId = alert.Id, 
                });
                await _context.SaveChangesAsync();
             
                _toast.AddSuccessToastMessage("Broadcast Sent");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _toast.AddErrorToastMessage("An error occured");

            }

            return Redirect("/tutor-announcement");
        }


    }
}
