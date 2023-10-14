using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using XeerLearn.Models.Auth;

namespace XeerLearn.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public readonly UserManager<XeerLearnUser> _manager;

        public IndexModel(ILogger<IndexModel> logger, UserManager<XeerLearnUser> manager)
        {
            _logger = logger;
            _manager = manager;
        }

        public async void OnGet()
        {
    
         /*  var user = User.Identity.Name != null ? await _manager.FindByEmailAsync(User.Identity.Name) : null;
                
            if(user != null)
            {
                var role = await _manager.GetRolesAsync(user);
                if (role[0] == "Tutor")
                   return Redirect($"/change-level/{user.Id}");
                return Redirect("/student-dashboard");
            }

            return Redirect("/");
*/
        }
    }
}