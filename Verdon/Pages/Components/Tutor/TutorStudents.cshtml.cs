using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Data;
using Verdon.Data;
using Verdon.Models.Auth;
using Verdon.Models.Utility;

namespace Verdon.Pages.Components.Tutor
{
    [Authorize(Roles = "Tutor")]
    public class TutorStudentsModel : PageModel
    {
        [BindProperty]
        public List<StudentTutor> students { get; set; }

        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;
        public readonly UserManager<VerdonUser> _manager;
        public VerdonUser user { get; set; }


        public TutorStudentsModel(UserManager<VerdonUser> manager, IToastNotification toast, ApplicationDbContext context)
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
                students = await _context.StudentTutors.Where(x => x.TutorId == user.Id).ToListAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
