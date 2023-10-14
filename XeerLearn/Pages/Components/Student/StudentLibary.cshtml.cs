using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Data;
using XeerLearn.Controllers;
using XeerLearn.Data;
using XeerLearn.Models.Auth;
using XeerLearn.Models.Libary;
using XeerLearn.Models.Utility;

namespace XeerLearn.Pages.Components.Student
{
    [Authorize(Roles = "Student")]
    public class StudentLibaryModel : PageModel
    {
        [BindProperty]
        public Libary material { get; set; }
        public Models.Auth.XeerLearnUser user { get; set; }
        [BindProperty]
        public string type { get; set; }
        [BindProperty]
        public string Id { get; set; }
        public Guid Access { get; set; }
        public List<Libary> materials { get; set; }
        public readonly ApplicationDbContext _context;
        public readonly IToastNotification _toast;
        public readonly UploadControllers _upload;
        public readonly UserManager<Models.Auth.XeerLearnUser> _manager;

        public StudentLibaryModel(UserManager<Models.Auth.XeerLearnUser> manager,UploadControllers upload,IToastNotification toast, ApplicationDbContext context)
        {
            _manager = manager;
            _upload = upload;
            _toast = toast;
            _context = context;

        }
        public async Task OnGet()
        {
            try
            {

                user = await _manager.FindByEmailAsync(User.Identity.Name);
                var access = await _context.UserAccess.Where(x => x.XeerLearnUserId == user.Id).FirstAsync();
                materials = await _context.Libaries.OrderByDescending(x=> x.Id).Where(x=> x.AccessKeyId == access.AccessKeyId).OrderByDescending(x => x.Id).ToListAsync();
                Access = access.AccessKeyId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
      

       
    }
}
