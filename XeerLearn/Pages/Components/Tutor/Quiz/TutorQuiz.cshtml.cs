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
    public class TutorQuizModel : PageModel
    {
        [BindProperty]
        public XeerLearn.Models.Utility.Quiz quiz { get; set; }
        [BindProperty]
        public UserAccess access { get; set; }
        [BindProperty]
        public List<XeerLearn.Models.Utility.Quiz> quizzes { get; set; }
        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;
        public readonly UserManager<Models.Auth.XeerLearnUser> _manager;
        public readonly IHttpContextAccessor _acc;
        public TutorQuizModel(IHttpContextAccessor acc,UserManager<Models.Auth.XeerLearnUser> manager,IToastNotification toast, ApplicationDbContext context)
        {
            _acc = acc;
            _manager = manager;
            _toast = toast;
            _context = context;

        }
        public async Task OnGet()
        {
            try
            {
                var user = await _manager.FindByEmailAsync(User.Identity.Name);
                var key = Guid.Parse(_acc.HttpContext.Session.GetString("AccessKey"));
                access = await _context.UserAccess.Where(x => x.AccessKeyId == key).FirstAsync();
                quizzes = await _context.Quiz.Where(x=> x.XeerLearnUserId == user.Id).OrderByDescending(x => x.Id).ToListAsync();
               
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
                quiz.AccessKeyId = (Guid.Parse(HttpContext.Session.GetString("AccessKey")));
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
