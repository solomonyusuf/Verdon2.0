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

namespace XeerLearn.Pages.Components.Student.Quiz
{
   

        [Authorize(Roles = "Student")]
        public class StudentResultModel : PageModel
        {
            [BindProperty]
            public QuizResult result { get; set; }

            public Models.Auth.XeerLearnUser user { get; set; }

            public readonly ApplicationDbContext _context;
            public readonly UserManager<Models.Auth.XeerLearnUser> _manager;
            public readonly IToastNotification _toast;


            public StudentResultModel(UserManager<Models.Auth.XeerLearnUser> manager, IToastNotification toast, ApplicationDbContext context)
            {
                _manager = manager;
                _toast = toast;
                _context = context;

            }


            public async Task OnGet(string Id)
            {
                try
                {
                    user = await _manager.FindByEmailAsync(User.Identity.Name);
                    result = await _context.QuizResults.Where(x => x.Id == Guid.Parse(Id)).FirstAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

        }
    }

