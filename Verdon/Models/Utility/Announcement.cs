namespace Verdon.Models.Utility
{
#nullable disable
    public class Announcement
    {
        public Guid Id { get; set; } 
        public string VerdonUserId { get; set; } 
        public Guid AccessKeyId { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }


        public Announcement()
        {
            DateTime = DateTime.Now;
        }
    }
}
