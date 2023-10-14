using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using XeerLearn.Data;
using XeerLearn.Models.Auth;
using XeerLearn.Models.Utility;

namespace XeerLearn.Pages.Components.Student.Quiz
{
    [Authorize(Roles = "Student")]
    public class StudentAttemptModel : PageModel
    {

        [BindProperty]
        public Models.Utility.Quiz quiz { get; set; }
        
        public string ResultId { get; set; }

        [BindProperty]
        public QuizResult result { get; set; }
        
        [BindProperty]
        public UserAccess access { get; set; }
        
        [BindProperty]
        public Models.Auth.XeerLearnUser user { get; set; }

        [BindProperty]
        public List<Question> questions { get; set; }

        public readonly ApplicationDbContext _context;
        public readonly UserManager<Models.Auth.XeerLearnUser> _manager;
        public readonly IToastNotification _toast;
        public readonly IHttpContextAccessor _acc;
        

        public StudentAttemptModel(IHttpContextAccessor acc,UserManager<Models.Auth.XeerLearnUser> manager, IToastNotification toast, ApplicationDbContext context)
        {
            _acc = acc;
            _manager = manager;
            _toast = toast;
            _context = context;

        }
         
          
        public async void OnGet(string Id)
        {
            try  
            {
                user = await _manager.FindByEmailAsync(User.Identity.Name);
                access = await _context.UserAccess.Where(x => x.XeerLearnUserId == user.Id).FirstAsync();
                quiz = await _context.Quiz.Where(x => x.Id == Guid.Parse(Id)).FirstAsync();
                questions = await _context.Questions.Where(x => x.QuizId == quiz.Id).ToListAsync();
               
              
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
                user = await _manager.FindByEmailAsync(User.Identity.Name);
                var form = Request.Form["options[]"].ToList();

                int score = 0;

                questions = await _context.Questions.Where(x => x.QuizId == quiz.Id).ToListAsync();

                for (int i = 0; i < questions.Count(); i++)
                {
                    if (form[i] == questions[i].Aplhabet)
                        ++score;
                }

                var input = new QuizResult
                {
                    AccessKeyId = Guid.Parse(Request.Form["access_key"]),
                    QuizId = quiz.Id,
                    StaticScore = int.Parse(Request.Form["static_score"]),
                    XeerLearnUserId = user.Id,
                    Name = user.FirstName+ " " + user.LastName,
                    Image = user.Image,
                    Score = score
                };

                var entity = await _context.QuizResults.AddAsync(input);
                await _context.SaveChangesAsync();

                ResultId = entity.Entity.Id.ToString();

                _toast.AddSuccessToastMessage("Submission Successful");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _toast.AddErrorToastMessage("An error occured");

            }

            return Redirect($"/student-result-{ResultId}");
        }



    }
}
