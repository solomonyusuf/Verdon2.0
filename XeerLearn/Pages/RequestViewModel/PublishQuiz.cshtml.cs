using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using XeerLearn.Data;

namespace XeerLearn.Pages.RequestViewModel
{
    public class PublishQuizModel : PageModel
    {
        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;

        public PublishQuizModel(IToastNotification toast, ApplicationDbContext context)
        {
            _toast = toast;
            _context = context;

        }
        public async Task<IActionResult> OnGet(string Id)
        {
            try
            {
                var quiz = await _context.Quiz.FindAsync(Guid.Parse(Id));
               
                if(quiz.Publish == true)
                    quiz.Publish = false;
                else
                    quiz.Publish = true;

                    _context.Quiz.Update(quiz); 
                await _context.SaveChangesAsync();
                _toast.AddSuccessToastMessage("Quiz Published Successfully");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Redirect($"/tutor-modifyquiz-{Id}");
        }
    }
}
