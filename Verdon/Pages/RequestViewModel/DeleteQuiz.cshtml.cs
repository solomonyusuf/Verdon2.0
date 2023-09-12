using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Verdon.Data;
using Verdon.Models.Utility;

namespace Verdon.Pages.RequestViewModel
{
    public class DeleteQuizModel : PageModel
    {
        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;

        public DeleteQuizModel(IToastNotification toast, ApplicationDbContext context)
        {
            _toast = toast;
            _context = context;

        }
        public async Task<IActionResult> OnGet(string Id)
        {
            try
            {

                var entity = await _context.Quiz.FindAsync(Guid.Parse(Id));
                _context.Quiz.Remove(entity);
                await _context.SaveChangesAsync();

                _toast.AddSuccessToastMessage("Deleted Sucessfully");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _toast.AddErrorToastMessage("An error occured");

            }

            return Redirect("/tutor-quiz");
        }
    }
}