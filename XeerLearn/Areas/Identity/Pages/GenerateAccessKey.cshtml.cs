using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using XeerLearn.Data;
using XeerLearn.Models.Auth;
using XeerLearn.Models.Utility;

namespace XeerLearn.Areas.Identity.Pages
{
    public class GenerateAccessKeyModel : PageModel
    {
        public readonly ApplicationDbContext _context;
        public readonly UserManager<XeerLearnUser> _manager;
        public readonly IToastNotification _toast;
        public GenerateAccessKeyModel(IToastNotification toast,ApplicationDbContext context, UserManager<XeerLearnUser> manager)
        {
            _toast = toast;
            _context = context;
            _manager = manager;
        }

        public XeerLearnUser user { get; set; }
        public string type { get; set; }

        [BindProperty]
        public AccessKey AccessKey { get; set; }
        
        [BindProperty]
        public UserAccess UserAccess { get; set; }


        public async Task OnGetAsync(string Id)
        {
            try
            {
                user = await _manager.FindByIdAsync(Id);
                if (user == null) Redirect("/");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if(type == "existing")
                {
                    await _context.UserAccess.AddAsync(UserAccess);
                    await _context.SaveChangesAsync();
                }

                if(type == "generate")
                {
                    var tracker =  await _context.AccessKeys.AddAsync(AccessKey);
                    UserAccess.AccessKeyId = tracker.Entity.Id;
                    UserAccess.ExpiryDate = tracker.Entity.ExpiryDate;
                    UserAccess.XeerLearnUserId = user.Id;
                    await _context.UserAccess.AddAsync(UserAccess);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            _toast.AddSuccessToastMessage("Access Key generated");
            
            if(User.IsInRole("Tutor"))
                 return Redirect("/tutor-dashboard");
            
            return Redirect("/student-dashboard");
        }
    }
}
