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

namespace XeerLearn.Pages.Components.Student
{
    [Authorize(Roles = "Student")]
    public class PeersModel : PageModel
    {
        [BindProperty]
        public List<UserAccess> students { get; set; }

        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;
        public readonly UserManager<Models.Auth.XeerLearnUser> _manager;
        public Models.Auth.XeerLearnUser user { get; set; }


        public PeersModel(UserManager<Models.Auth.XeerLearnUser> manager, IToastNotification toast, ApplicationDbContext context)
        {
            _manager = manager;
            _toast = toast;
            _context = context;

        }
        public async Task OnGet()
        {
            try
            {
                user = await _manager.FindByEmailAsync(User.Identity.Name);
                var access = await _context.UserAccess.Where(x => x.XeerLearnUserId == user.Id).FirstAsync();
                students = await _context.UserAccess.Where(x => x.AccessKeyId == access.AccessKeyId && x.Type == "Student").ToListAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
