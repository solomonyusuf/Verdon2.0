using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Verdon.Pages.Shared
{
    public class _MainLayoutModel : PageModel
    {
        public IActionResult SearchTutor()
        {
            return Redirect($"/tutor/search/{Request.Form["value"]}");
        }
    }
}
