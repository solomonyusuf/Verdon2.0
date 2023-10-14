using System.ComponentModel.DataAnnotations;

namespace XeerLearn.Models.Libary
{
#nullable disable
    public class Libary
    {
        [Key]
        public Guid Id { get; set; }
        public Guid AccessKeyId { get; set; }
        public string XeerLearnUserId { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string File { get; set; }
        public DateTime DateTime { get; set; }


        public Libary()
        {
            DateTime = DateTime.Now;
        }
    }
}
