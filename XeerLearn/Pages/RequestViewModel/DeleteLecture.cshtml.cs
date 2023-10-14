using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using XeerLearn.Controllers;
using XeerLearn.Data;
using XeerLearn.Models.Utility;

namespace XeerLearn.Pages.RequestViewModel
{
    public class DeleteLectureModel : PageModel
    {
        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;

        public DeleteLectureModel(IToastNotification toast, ApplicationDbContext context)
        {
            _toast = toast;
            _context = context;

        }
        public async Task<IActionResult> OnGet(string Id)
        {
            try
            {

                var entity = await _context.Meetings.FindAsync(Guid.Parse(Id));
                _context.Meetings.Remove(entity);
                await _context.SaveChangesAsync();

                _toast.AddSuccessToastMessage("Deleted Sucessfully");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _toast.AddErrorToastMessage("An error occured");

            }

            return Redirect("/tutor-lectures");
        }
    }
}
