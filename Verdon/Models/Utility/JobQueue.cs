using System.ComponentModel.DataAnnotations;

namespace Verdon.Models.Utility
{
    public class JobQueue
    {
        [Key]
        public Guid Id { get; set; }
        public bool Terminated { get; set; }
        public string Type { get; set; }
        public Guid AccessKeyId { get; set; }
        public Guid ConstrainId { get; set; }
    }
}
