using System.ComponentModel.DataAnnotations;

namespace Verdon.Models.Utility
{
    public class Course
    {
        [Key]
        public Guid Id { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public long Description { get; set; }
        public DateTime DateTime { get; set; }


        public Course()
        {
            Id = Guid.Parse(Guid.NewGuid().ToString("N"));
            DateTime = DateTime.Now;
        }
    }
}
