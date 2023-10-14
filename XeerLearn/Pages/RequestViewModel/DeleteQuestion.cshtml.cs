using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using XeerLearn.Data;
using XeerLearn.Models.Utility;

namespace XeerLearn.Pages.RequestViewModel
{
    public class DeleteQuestionModel : PageModel
    {
        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;
        public Question question { get; set; }
        public DeleteQuestionModel(IToastNotification toast, ApplicationDbContext context)
        {
            _toast = toast;
            _context = context;

        }
        public async Task<IActionResult> OnGet(string Id)
        {
            try
            {

                question = await _context.Questions.FindAsync(Guid.Parse(Id));
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();

                _toast.AddSuccessToastMessage("Deleted Sucessfully");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _toast.AddErrorToastMessage("An error occured");

            }

            return Redirect($"/tutor-modifyquiz-{question.QuizId}");
        }
    }
}