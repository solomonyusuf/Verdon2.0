using System.ComponentModel.DataAnnotations;

namespace Verdon.Models.Utility
{
#nullable disable
    public class AssignmentResult
    {
        [Key]
        public Guid Id { get; set; }
        public Guid AccessKeyId { get; set; }
        public Guid AssignmentId { get; set; }
        public string VerdonUserId { get; set; }
        public double Score { get; set; }
        public double TotalScore { get; set; }
        public DateTime DateTime { get; set; }


        public AssignmentResult()
        {
            Id = Guid.Parse(Guid.NewGuid().ToString("N"));
            DateTime = DateTime.Now;
        }
    }
}
