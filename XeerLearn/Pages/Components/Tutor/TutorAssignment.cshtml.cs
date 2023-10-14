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
    public class TutorAssignmentModel : PageModel
    {
        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;
        public readonly UserManager<Models.Auth.XeerLearnUser> _manager;
        public List<Assignment> assignments { get;set; }
        [BindProperty]
        public Assignment assignment { get;set; }
        public Guid Access { get;set; }
        public readonly IHttpContextAccessor _acc;

        public TutorAssignmentModel(IHttpContextAccessor acc, UserManager<Models.Auth.XeerLearnUser> manager,IToastNotification toast, ApplicationDbContext context)
        {
            _acc = acc;
            _manager = manager;
            _toast = toast;
            _context = context;

        }
        public async Task OnGet()
        {
            var user = await _manager.FindByEmailAsync(User.Identity.Name);
            var access = await _context.UserAccess.Where(x => x.AccessKeyId == Guid.Parse(_acc.HttpContext.Session.GetString("AccessKey"))).FirstAsync();
            assignments = await _context.Assignments.Where(x=> x.XeerLearnUserId == user.Id).OrderByDescending(x=> x.Id).ToListAsync();
            Access = access.AccessKeyId;
        }
        public async Task<IActionResult> OnPost()
        {
            try
            {
                
                await _context.Assignments.AddAsync(assignment);
                await _context.SaveChangesAsync();
                _toast.AddSuccessToastMessage("New Assignment Created");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _toast.AddErrorToastMessage("An error occured");

            }

            return Redirect("/tutor-assignments");
        }
    }
}
