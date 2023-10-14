using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using XeerLearn.Models.Auth;
using XeerLearn.Models.Blog;
using XeerLearn.Models.Libary;
using XeerLearn.Models.Money;
using XeerLearn.Models.Utility;

namespace XeerLearn.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public virtual DbSet<Models.Auth.XeerLearnUser> User { get; set; }
        public virtual DbSet<XeerLearnRole> Role { get; set; }
        public virtual DbSet<XeerLearnUserRole> UserRole { get; set; }
        public virtual DbSet<StudentTutor> StudentTutors { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<Wallet> Wallets { get; set; }
        public virtual DbSet<Announcement> Announcements { get; set; }
        public virtual DbSet<Assignment> Assignments { get; set; }
        public virtual DbSet<AssignmentResponse> AssignmentResponses { get; set; }
        public virtual DbSet<AssignmentResult> AssignmentResults { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseCollection> CourseCollections { get; set; }
        public virtual DbSet<Credential> Credentials { get; set; }
        public virtual DbSet<Meeting> Meetings { get; set; }
        public virtual DbSet<Option> Options { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderHistory> OrderHistories { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Quiz> Quiz { get; set; }
        public virtual DbSet<QuizResult> QuizResults { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<ArticleCategory> ArticleCategories { get; set; }
        public virtual DbSet<Libary> Libaries { get; set; }
        public virtual DbSet<AccessKey> AccessKeys { get; set; }
        public virtual DbSet<UserAccess> UserAccess { get; set; }
        public virtual DbSet<JobQueue> JobQueue { get; set; }
 
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {


/*            if (Database.EnsureCreatedAsync().IsCompletedSuccessfully)
            {
                if (Database.GetPendingMigrationsAsync().Result.Count() > 0)
                    Database.MigrateAsync();
            }*/
        }
    }
}