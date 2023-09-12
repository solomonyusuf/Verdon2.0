using System.ComponentModel.DataAnnotations;

namespace Verdon.Models.Utility
{
#nullable disable
    public class Credential
    {
        [Key]
        public Guid Id { get; set; }
        public string VerdonUserId { get; set; }
        public string Title { get; set; }
        public string File { get; set; }
        public DateTime DateTime { get; set; }


        public Credential()
        {
            Id = Guid.Parse(Guid.NewGuid().ToString("N"));
            DateTime = DateTime.Now;
        }
    }
}
