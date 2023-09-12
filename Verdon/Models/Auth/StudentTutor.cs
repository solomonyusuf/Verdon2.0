using System.ComponentModel.DataAnnotations;

namespace Verdon.Models.Auth
{  
    #nullable disable
    public class StudentTutor
    {
        [Key]
        public string TutorId { get; set; }
        public string StudentId { get; set; }
        
    }
}
