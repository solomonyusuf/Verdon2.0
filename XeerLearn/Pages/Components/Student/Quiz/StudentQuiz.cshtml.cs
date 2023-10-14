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

namespace XeerLearn.Pages.Components.Student
{
    [Authorize(Roles = "Student")]
    public class StudentQuizModel : PageModel
    {
        [BindProperty]
        public XeerLearn.Models.Utility.Quiz quiz { get; set; }
        
        [BindProperty]
        public XeerLearnUser user { get; set; }
        public List<XeerLearn.Models.Utility.Quiz> quizzes { get; set; }
        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;
        public readonly UserManager<Models.Auth.XeerLearnUser> _manager;

        public StudentQuizModel(UserManager<Models.Auth.XeerLearnUser> manager,IToastNotification toast, ApplicationDbContext context)
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
                var access = await _context.UserAccess.Where(x => x.XeerLearnUserId == user.Id).FirstAsync();
                quizzes = await _context.Quiz.Where(x=> x.AccessKeyId == access.AccessKeyId).Where(x=> x.Publish == true).OrderByDescending(x => x.Id).ToListAsync();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
       

       

    }
}
