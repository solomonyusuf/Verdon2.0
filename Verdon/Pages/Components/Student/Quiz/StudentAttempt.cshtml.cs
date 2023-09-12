using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Verdon.Data;
using Verdon.Models.Utility;

namespace Verdon.Pages.Components.Student.Quiz
{
    [Authorize(Roles = "Student")]
    public class StudentAttemptModel : PageModel
    {

        [BindProperty]
        public Verdon.Models.Utility.Quiz quiz { get; set; }
        [BindProperty]
        public Question question { get; set; }
        
        [BindProperty]
        public List<QuizResult> result { get; set; }
       
        public List<Question> questions { get; set; }
        [BindProperty]
        public string option_1 { get; set; }
        [BindProperty]
        public string option_2 { get; set; }
        [BindProperty]
        public string option_3 { get; set; }
        [BindProperty]
        public string option_4 { get; set; }
        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;

        public StudentAttemptModel(IToastNotification toast, ApplicationDbContext context)
        {
            _toast = toast;
            _context = context;

        }
        public async Task OnGet(string Id)
        {
            try
            {
                quiz = await _context.Quiz.Where(x => x.Id == Guid.Parse(Id)).FirstAsync();
                questions = await _context.Questions.Where(x => x.QuizId == quiz.Id).ToListAsync();
                result = await _context.QuizResults.Where(x => x.QuizId == quiz.Id).ToListAsync();
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
                var entity = await _context.Questions.AddAsync(question);
                await _context.SaveChangesAsync();
                AddOption(quiz.Id,question.Id, "A", option_1);
                AddOption(quiz.Id, question.Id, "B", option_2);
                AddOption(quiz.Id, question.Id, "C", option_3);
                AddOption(quiz.Id, question.Id, "D", option_4);
                await _context.SaveChangesAsync();
                _toast.AddSuccessToastMessage("New Question Created");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _toast.AddErrorToastMessage("An error occured");

            }

            return Redirect($"/tutor-modifyquiz-{quiz.Id}");
        }

        async void AddOption (Guid Id, Guid q_Id, string alphabet, string option)
        {
            await _context.Options.AddAsync(new Option { Alphabet = alphabet, QuestionId = q_Id, Content = option, QuizId = Id });
        }



    }
}
