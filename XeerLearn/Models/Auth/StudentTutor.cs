using System.ComponentModel.DataAnnotations;

namespace XeerLearn.Models.Auth
{  
    #nullable disable
    public class StudentTutor
    {
        [Key]
        public string TutorId { get; set; }
        public string StudentId { get; set; }
        
    }
}
