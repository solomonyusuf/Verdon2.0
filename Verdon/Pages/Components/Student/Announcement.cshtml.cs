using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Verdon.Controllers;
using Verdon.Data;
using Verdon.Models.Auth;
using Verdon.Models.Utility;

namespace Verdon.Pages.Components.Student
{
    public class AnnouncementModel : PageModel
    {
        [BindProperty]
        public Announcement alert { get; set; }
        public VerdonUser user { get; set; }
        [BindProperty]
        public Guid Access { get; set; }
        public List<Announcement> alerts { get; set; }
        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;
        public readonly StreamController _stream;
        public readonly IConfiguration _config;
        public readonly UserManager<VerdonUser> _manager;

        public AnnouncementModel(UserManager<VerdonUser> manager, IConfiguration config, StreamController stream, IToastNotification toast, ApplicationDbContext context)
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
                user = await _manager.FindByEmailAsync(User.Identity.Name);
                var access = await _context.UserAccess.Where(x => x.VerdonUserId == user.Id).FirstAsync();

                alerts = await _context.Announcements.Where(x => x.AccessKeyId == access.AccessKeyId).OrderByDescending(x => x.Id).ToListAsync();
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }
}
