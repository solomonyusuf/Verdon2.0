using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using XeerLearn.Models.Auth;

namespace XeerLearn.Areas.Identity.Pages.Account
{
    public class ConfirmAccountModel : PageModel
    {

        private readonly UserManager<Models.Auth.XeerLearnUser> _userManager;
        private readonly IToastNotification _toast;

        public ConfirmAccountModel(UserManager<Models.Auth.XeerLearnUser> userManager, IToastNotification toast)
        {
            _userManager = userManager;
            _toast = toast;
        }

        public async Task<IActionResult> OnGetAsync(string Id)
        {
            if (Id == null)
            {
                return Redirect("/");
            }

            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{Id}'.");
            }
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var result = await _userManager.ConfirmEmailAsync(user, code);
            var role = await _userManager.GetRolesAsync(user);

            _toast.AddSuccessToastMessage("Account Confirmed");

            if (role[0] == "student")
                    return LocalRedirect("/become-student");
            if (role[0] == "tutor")
                return LocalRedirect("/become-tutor");
            
            return LocalRedirect("/");
        }
    }
}
