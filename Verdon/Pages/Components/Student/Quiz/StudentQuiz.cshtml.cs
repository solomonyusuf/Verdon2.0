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

namespace Verdon.Pages.Components.Student
{
    [Authorize(Roles = "Student")]
    public class StudentQuizModel : PageModel
    {
        [BindProperty]
        public Verdon.Models.Utility.Quiz quiz { get; set; }
        public List<Verdon.Models.Utility.Quiz> quizzes { get; set; }
        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;
        public readonly UserManager<VerdonUser> _manager;

        public StudentQuizModel(UserManager<VerdonUser> manager,IToastNotification toast, ApplicationDbContext context)
        {
            _manager = manager;
            _toast = toast;
            _context = context;

        }
        public async Task OnGet()
        {
            try
            {
                var user = await _manager.FindByEmailAsync(User.Identity.Name);
                var access = await _context.UserAccess.Where(x => x.VerdonUserId == user.Id).FirstAsync();
                quizzes = await _context.Quiz.Where(x=> x.AccessKeyId == access.AccessKeyId).OrderByDescending(x => x.Id).ToListAsync();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
       

       

    }
}
