using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using XeerLearn.Data;

namespace XeerLearn.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "Tutor")]
    public class TutorChangeLevelModel : PageModel
    {
        public readonly ApplicationDbContext _context;
        private readonly IToastNotification _toast;
        private readonly UserManager<Models.Auth.XeerLearnUser> _userManager;
        public readonly IHttpContextAccessor _acc;
        public TutorChangeLevelModel(ApplicationDbContext context,
            IToastNotification toast,
            UserManager<Models.Auth.XeerLearnUser> userManager,
             IHttpContextAccessor acc)
        {
            _context = context;
            _toast = toast;
            _userManager = userManager;
            _acc = acc;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync(string Id)
        {
            try
            {

                var input = Request.Form["level"].ToString();
                if (input == null)
                {
                    _toast.AddErrorToastMessage("Select a level to tutor");
                    return Redirect($"/change-level/{Id}");
                }
                
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                var access = await _context.UserAccess.Where(x => x.AccessKeyId == Guid.Parse(input) && x.XeerLearnUserId == user.Id).SingleOrDefaultAsync();
                _acc.HttpContext.Session.SetString("AccessKey", access.AccessKeyId.ToString());

                _toast.AddAlertToastMessage($"Welcome {user.FirstName} {user.LastName}");

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Redirect("/tutor-dashboard");
        }
    }
}
