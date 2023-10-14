using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using XeerLearn.Controllers;
using XeerLearn.Data;

namespace XeerLearn.Pages.Components.Lecture
{
    public class AttendeeModel : PageModel
    {
        public readonly StreamController _stream;
        public readonly ApplicationDbContext _context;
        public readonly UserManager<Models.Auth.XeerLearnUser> _manager;
        public AttendeeModel(UserManager<Models.Auth.XeerLearnUser> manager, ApplicationDbContext context, StreamController stream)
        {
            _manager = manager;
            _context = context;
            _stream = stream;
        }
        public async Task<IActionResult> OnGet(string Id)
        {
            var user = await _manager.FindByEmailAsync(User.Identity.Name);
            var meeting = await _context.Meetings.FindAsync(Guid.Parse(Id));
            var url = _stream.StudentJoinMeeting(meeting, user);

            return Redirect(url);
        }
    }
}
