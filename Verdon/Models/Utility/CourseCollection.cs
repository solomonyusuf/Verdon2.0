using System.ComponentModel.DataAnnotations;

namespace Verdon.Models.Utility
{
#nullable disable
    public class CourseCollection
    {
        [Key]
        public Guid Id { get; set; }
        public string VerdonUserId { get; set; }
        public string CourseId { get; set; }
        public DateTime DateTime { get; set; }


        public CourseCollection()
        {
            Id = Guid.Parse(Guid.NewGuid().ToString("N"));
            DateTime = DateTime.Now;
        }
    }
}
