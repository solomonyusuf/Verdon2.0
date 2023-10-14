using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using XeerLearn.Data;
using XeerLearn.Models.Utility;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace XeerLearn.Pages.Components.Tutor.Quiz
{
    [Authorize(Roles = "Tutor")]
    public class TutorEditQuestionModel : PageModel
    {
        [BindProperty]
        public Question question { get; set; }
        [BindProperty]
        public List<Option> options { get; set; }
        [BindProperty]
        public XeerLearn.Models.Utility.Quiz quiz { get; set; }
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

        public TutorEditQuestionModel(IToastNotification toast, ApplicationDbContext context)
        {
            _toast = toast;
            _context = context;

        }
        public async Task OnGet(string Id)
        {
            try
            {
                question = await _context.Questions.Where(x => x.Id == Guid.Parse(Id)).FirstAsync();
                quiz = await _context.Quiz.Where(x => x.Id == question.QuizId).FirstAsync();
                options = await _context.Options.Where(x => x.QuestionId == question.Id)
                            .OrderBy(x => x.Alphabet).ToListAsync();


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
                var entity = _context.Questions.Update(question);
                await _context.SaveChangesAsync();

         
                AddOption(options[0].Id,quiz.Id, question.Id, "A", option_1);
                AddOption(options[0].Id, quiz.Id, question.Id, "B", option_2);
                AddOption(options[0].Id, quiz.Id, question.Id, "C", option_3);
                AddOption(options[0].Id, quiz.Id, question.Id, "D", option_4);
                await _context.SaveChangesAsync();
                _toast.AddSuccessToastMessage("Question Updated");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _toast.AddErrorToastMessage("An error occured");

            }

            return Redirect($"/tutor-editquestion-{question.Id}");
        }

        void AddOption(Guid key, Guid Id, Guid q_Id, string alphabet, string option)
        {
           _context.Options.Update(new Option {Id = key, Alphabet = alphabet, QuestionId = q_Id, Content = option, QuizId = Id });
        }



    }
}
