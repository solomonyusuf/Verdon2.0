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
    public class TutorQuizModel : PageModel
    {
        [BindProperty]
        public Verdon.Models.Utility.Quiz quiz { get; set; }
        public List<Verdon.Models.Utility.Quiz> quizzes { get; set; }
        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;
        public readonly UserManager<VerdonUser> _manager;

        public TutorQuizModel(UserManager<VerdonUser> manager,IToastNotification toast, ApplicationDbContext context)
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
                quizzes = await _context.Quiz.Where(x=> x.VerdonUserId == user.Id).OrderByDescending(x => x.Id).ToListAsync();
                quiz.AccessKeyId = access.AccessKeyId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public async Task<IActionResult> OnPost()
        {
            try
            {
                await _context.Quiz.AddAsync(quiz);
                await _context.SaveChangesAsync();
                _toast.AddSuccessToastMessage("New Quiz Created.Proceed to add questions.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _toast.AddErrorToastMessage("An error occured");

            }

            return Redirect($"/tutor-modifyquiz-{quiz.Id}");
        }

       

    }
}
