using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using XeerLearn.Data;
using XeerLearn.Models.Auth;
using XeerLearn.Models.Money;

namespace XeerLearn.Pages.Components.Tutor.Payment
{
    [Authorize(Roles = "Tutor")]
    public class TutorTransactionModel : PageModel
    {
        [BindProperty]
        public List<Transaction> transactions { get; set; }

        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;
        public readonly UserManager<Models.Auth.XeerLearnUser> _manager;
        public Models.Auth.XeerLearnUser user { get; set; }


        public TutorTransactionModel(UserManager<Models.Auth.XeerLearnUser> manager, IToastNotification toast, ApplicationDbContext context)
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
                var access = await _context.UserAccess.Where(x => x.AccessKeyId == Guid.Parse(HttpContext.Session.GetString("AccessKey"))).FirstAsync();
                transactions = await _context.Transactions.Where(x => x.AccessKeyId == access.AccessKeyId).OrderByDescending(x=> x.Id).ToListAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}