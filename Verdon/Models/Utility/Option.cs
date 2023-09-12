using System.ComponentModel.DataAnnotations;

namespace Verdon.Models.Utility
{
#nullable disable
    public class Option
    {
        [Key]
        public Guid Id { get; set; }
        public Guid QuizId { get; set; }
        public Guid QuestionId { get; set; }
        public string Alphabet { get; set; }
        public string Content { get; set; }
        public DateTime DateTime { get; set; }


        public Option()
        {
            Id = Guid.Parse(Guid.NewGuid().ToString("N"));
            DateTime = DateTime.Now;
        }
    }
}
