using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Data;
using XeerLearn.Data;
using XeerLearn.Models.Auth;
using XeerLearn.Models.Utility;

namespace XeerLearn.Pages.Components.Tutor
{
    [Authorize(Roles = "Tutor")]
    public class TutorStudentsModel : PageModel
    {
        [BindProperty]
        public List<UserAccess> students { get; set; }

        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;
        public readonly UserManager<Models.Auth.XeerLearnUser> _manager;
        public Models.Auth.XeerLearnUser user { get; set; }
        public readonly IHttpContextAccessor _acc;

        public TutorStudentsModel(IHttpContextAccessor acc,UserManager<Models.Auth.XeerLearnUser> manager, IToastNotification toast, ApplicationDbContext context)
        {
            _acc = acc;
            _manager = manager;
            _toast = toast;
            _context = context;

        }
        public async Task OnGet()
        {
            try
            {
                user = await _manager.FindByEmailAsync(User.Identity.Name);
                students = await _context.UserAccess.Where(x => x.AccessKeyId == Guid.Parse(_acc.HttpContext.Session.GetString("AccessKey"))).ToListAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
