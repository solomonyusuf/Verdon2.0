using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Verdon.Data;
using Verdon.Models.Auth;
using Verdon.Models.Money;

namespace Verdon.Pages.Components.Tutor.Payment
{
    [Authorize(Roles = "Tutor")]
    public class TutorTransactionModel : PageModel
    {
        [BindProperty]
        public List<Transaction> transactions { get; set; }

        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;
        public readonly UserManager<VerdonUser> _manager;
        public VerdonUser user { get; set; }


        public TutorTransactionModel(UserManager<VerdonUser> manager, IToastNotification toast, ApplicationDbContext context)
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
                var access = await _context.UserAccess.Where(x => x.VerdonUserId == user.Id).FirstAsync();
                transactions = await _context.Transactions.Where(x => x.AccessKeyId == access.AccessKeyId).OrderByDescending(x=> x.Id).ToListAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}