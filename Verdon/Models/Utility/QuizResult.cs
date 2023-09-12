using System.ComponentModel.DataAnnotations;

namespace Verdon.Models.Utility
{
#nullable disable
    public class QuizResult
    {
        [Key]
        public Guid Id { get; set; }
        public Guid AccessKeyId { get; set; }
        public Guid QuizId { get; set; }
        public string VerdonUserId { get; set; }
        public double Score { get; set; }
        public double StaticScore { get; set; }
        public DateTime DateTime { get; set; }


        public QuizResult()
        {
            Id = Guid.Parse(Guid.NewGuid().ToString("N"));
            DateTime = DateTime.Now;
        }
    }
}
