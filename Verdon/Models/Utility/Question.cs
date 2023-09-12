using System.ComponentModel.DataAnnotations;

namespace Verdon.Models.Utility
{
#nullable disable
    public class Question
    {
        [Key]
        public Guid Id { get; set; }
        public Guid QuizId { get; set; }
        public string Aplhabet { get; set; }
        public string Content { get; set; }
        public DateTime DateTime { get; set; }


        public Question()
        {
            Id = Guid.Parse(Guid.NewGuid().ToString("N"));
            DateTime = DateTime.Now;
        }
    }
}
