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

namespace XeerLearn.Pages.Components.Student.AssignmentTask
{
    [Authorize(Roles = "Student")]
    public class StudentAssignmentModel : PageModel
    {
        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;
        public readonly UserManager<Models.Auth.XeerLearnUser> _manager;
        public List<XeerLearn.Models.Utility.Assignment> assignments { get;set; }
        [BindProperty]
        public XeerLearn.Models.Utility.Assignment assignment { get;set; }
        public Guid Access { get;set; }

        public StudentAssignmentModel(UserManager<Models.Auth.XeerLearnUser> manager,IToastNotification toast, ApplicationDbContext context)
        {
            _manager = manager;
            _toast = toast;
            _context = context;

        }
        public async Task OnGet()
        {
            var user = await _manager.FindByEmailAsync(User.Identity.Name);
            var access = await _context.UserAccess.Where(x => x.XeerLearnUserId == user.Id).FirstAsync();
            assignments = await _context.Assignments.Where(x=> x.AccessKeyId == access.AccessKeyId)
                .Where(x => x.Published == true)
                .OrderByDescending(x=> x.Id).ToListAsync();
          
        }
    }
}
