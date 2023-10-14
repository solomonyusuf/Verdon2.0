using System.ComponentModel.DataAnnotations;

namespace XeerLearn.Models.Utility
{
#nullable disable
    public class QuizResult
    {
        [Key]
        public Guid Id { get; set; }
        public Guid AccessKeyId { get; set; }
        public Guid QuizId { get; set; }
        public string XeerLearnUserId { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
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
