using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Data;
using Verdon.Data;
using Verdon.Models.Utility;

namespace Verdon.Pages.Components.Tutor
{
    [Authorize(Roles = "Tutor")]
    public class TutorModifyAssignmentModel : PageModel
    {

        [BindProperty]
        public Assignment assignment { get; set; }

        [BindProperty]
        public bool publish { get; set; }

        [BindProperty]
        public string type { get; set; }

        [BindProperty]
        public AssignmentResult result { get; set; }
        
        [BindProperty]
        public List<AssignmentResponse> responses { get; set; }
      
        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;

        public TutorModifyAssignmentModel(IToastNotification toast, ApplicationDbContext context)
        {
            _toast = toast;
            _context = context;

        }
        public async Task OnGet(string Id)
        {
            try
            {
                assignment = await _context.Assignments.FindAsync(Guid.Parse(Id));
                responses = await _context.AssignmentResponses.Where(x => x.AssignmentId == assignment.Id).ToListAsync();
                
               
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
               if(type == "assignment")
                {
                    _context.Assignments.Update(assignment);
                    await _context.SaveChangesAsync();
                    _toast.AddSuccessToastMessage("Updated Assignment");
                    type = String.Empty;

                }
                if(type == "grade")
                {
                    await _context.AssignmentResults.AddAsync(result);
                    await _context.SaveChangesAsync();
                    _toast.AddSuccessToastMessage("Assignment graded successful");
                    type = String.Empty;
                }
                if(type == "publishing")
                {
                    var entity = await _context.Assignments.FindAsync(assignment.Id);
                    entity.Published = publish;
                    _context.Assignments.Update(entity);
                    await _context.SaveChangesAsync();

                    _toast.AddSuccessToastMessage("Assignment Published Successfully");
                    type = String.Empty;
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _toast.AddErrorToastMessage("An error occured");

            }

            return Redirect($"/tutor-modifyassignment-{assignment.Id}");
        }

        async void AddOption (Guid Id, Guid q_Id, string alphabet, string option)
        {
            await _context.Options.AddAsync(new Option { Alphabet = alphabet, QuestionId = q_Id, Content = option, QuizId = Id });
        }



    }
}
