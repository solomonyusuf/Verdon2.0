using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Verdon.Data;
using Verdon.Models.Auth;
using Verdon.Models.Libary;
using Verdon.Models.Utility;

namespace Verdon.Pages.Components.Tutor
{
    [Authorize(Roles = "Tutor")]
    public class TutorDashboardModel : PageModel
    {
        public readonly ApplicationDbContext _context;
        public readonly UserManager<VerdonUser> _manager;
        public List<UserAccess> students { get; set; }
        public List<Assignment> assignment { get; set; }
        public List<Verdon.Models.Utility.Quiz> quiz { get; set; }
        public List<Meeting> lectures { get; set; }
        public List<Review> reviews { get; set; }
        public List<Announcement> announcements { get; set; }
        public List<Libary> libary { get; set; }
        public int Result = 0;


        public TutorDashboardModel(ApplicationDbContext context, UserManager<VerdonUser> manager)
        {
            _context = context;
            _manager = manager;
        }
        public async void OnGet()
        {
            try
            {
                var user = await _manager.FindByEmailAsync(User.Identity.Name);
                var access = await _context.UserAccess.Where(x => x.VerdonUserId == user.Id).FirstAsync();
                students = await _context.UserAccess.Where(x => x.Type == "student").Where(x => x.AccessKeyId == access.AccessKeyId).ToListAsync();
                assignment = await _context.Assignments.Where(x => x.VerdonUserId == user.Id).ToListAsync();
                quiz = await _context.Quiz.Include(x => x.Results).Where(x => x.VerdonUserId == user.Id).ToListAsync();
                lectures = await _context.Meetings.Where(x => x.VerdonUserId == user.Id).ToListAsync();
                reviews = await _context.Reviews.Where(x => x.VerdonUserId == user.Id).ToListAsync();
                announcements = await _context.Announcements.Where(x => x.VerdonUserId == user.Id).ToListAsync();
                libary = await _context.Libaries.Where(x => x.VerdonUserId == user.Id).ToListAsync();
                quiz.ForEach(x => Result += x.Results.Count());
            
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
     
    }
}
