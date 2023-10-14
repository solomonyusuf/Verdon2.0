using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace XeerLearn.Models.Utility
{
    public class AccessKey
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Students { get; set; }
        public int Tutors { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime DateTime { get; set; }


        public AccessKey()
        {
            Id = Guid.Parse(Guid.NewGuid().ToString("N"));
            DateTime = DateTime.Now;
            
        }
    }
}
