using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using NToastNotify.Helpers;
using XeerLearn.Data;
using XeerLearn.Models.Auth;
using XeerLearn.Models.Utility;

namespace XeerLearn.Pages
{
    public class ReviewModel : PageModel
    {
        [BindProperty]
        public List<Review> reviews { get; set; }

        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;
        public readonly UserManager<Models.Auth.XeerLearnUser> _manager;
        public Models.Auth.XeerLearnUser user { get; set; }


        public ReviewModel(UserManager<Models.Auth.XeerLearnUser> manager,IToastNotification toast, ApplicationDbContext context)
        {
            _manager = manager;
            _toast = toast;
            _context = context;

        }
        public async Task OnGet()
        {
            try
            {
                user = await _manager.FindByEmailAsync(User.Identity.Name);
                reviews = await _context.Reviews.Where(x=> x.TutorId == user.Id).OrderByDescending(x => x.Id).ToListAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
