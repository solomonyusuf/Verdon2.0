using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Data;
using Verdon.Data;
using Verdon.Models.Auth;

namespace Verdon.Pages.Components.Tutor
{
    [Authorize(Roles = "Tutor")]
    public class AISearchModel : PageModel
    {
        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;
        public List<VerdonUser> Tutor { get; set; }

        public AISearchModel(ApplicationDbContext context, IToastNotification toast)
        {
            _context = context;
            _toast = toast;
        }
        public async Task OnGetAsync(string value)
        {
            try
            {
                Tutor = await _context.User.Where(x => x.Type.Equals("tutor"))
                 .Where(x => x.FirstName.Contains(value) || x.LastName.Contains(value) || x.Email.Contains(value))
                 .ToListAsync();

                if (Tutor.Count > 0)
                    _toast.AddSuccessToastMessage("Result fetched successfully");
                else
                    _toast.AddWarningToastMessage("No result found");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
         
    }
}
