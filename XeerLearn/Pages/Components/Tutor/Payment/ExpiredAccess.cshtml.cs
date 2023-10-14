using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace XeerLearn.Pages.Components
{
    [Authorize(Roles = "Tutor")]
    public class ExpiredAccessModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
