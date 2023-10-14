using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Data;
using XeerLearn.Data;
using XeerLearn.Models.Utility;

namespace XeerLearn.Pages.Components.Student.AssignmentTask
{
    [Authorize(Roles = "Student")]
    public class StudentModifyAssignmentModel : PageModel
    {

        [BindProperty]
        public Assignment assignment { get; set; }
        
        [BindProperty]
        public AssignmentResponse response { get; set; }
      
        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;

        public StudentModifyAssignmentModel(IToastNotification toast, ApplicationDbContext context)
        {
            _toast = toast;
            _context = context;

        }
        public async Task OnGet(string Id)
        {
            try
            {
                assignment = await _context.Assignments.FindAsync(Guid.Parse(Id));
               
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
             
                    await _context.AssignmentResponses.AddAsync(response);
                    await _context.SaveChangesAsync();
                    _toast.AddSuccessToastMessage("Assignment Submitted");
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _toast.AddErrorToastMessage("An error occured");

            }

            return Redirect($"/student-modifyassignment-{assignment.Id}");
        }


    }
}
