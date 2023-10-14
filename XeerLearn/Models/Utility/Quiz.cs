using System.ComponentModel.DataAnnotations;

namespace XeerLearn.Models.Utility
{
#nullable disable
    public class Quiz
    {
        [Key]
        public Guid Id { get; set; }
        public Guid AccessKeyId { get; set; }
        public string XeerLearnUserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Publish { get; set; }
        public double Duration { get; set; }
        public double PointScore { get; set; }
        public List<QuizResult> Results { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime DateTime { get; set; }


        public Quiz()
        {
            DateTime = DateTime.Now;
            Publish = false;
            Results = new List<QuizResult>();
        }
    }
}
