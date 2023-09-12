using System.ComponentModel.DataAnnotations;

namespace Verdon.Models.Utility
{
#nullable disable
    public class AssignmentResponse
    {
        [Key]
        public Guid Id { get; set; }
        public Guid AccessKeyId { get; set; }
        public Guid AssignmentId { get; set; }
        public string VerdonUserId { get; set; }
        public long Response { get; set; }
        public DateTime DateTime { get; set; }


        public AssignmentResponse()
        {
            Id = Guid.Parse(Guid.NewGuid().ToString("N"));
            DateTime = DateTime.Now;
        }
    }
}
