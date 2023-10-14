using System.ComponentModel.DataAnnotations;

namespace XeerLearn.Models.Utility
{
#nullable disable
    public class Assignment
    {
        [Key]
        public Guid Id { get; set; }
        public Guid AccessKeyId { get; set; }
        public string XeerLearnUserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TotalScore { get; set; }
        public bool Published { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime DateTime { get; set; }


        public Assignment()
        {
            Published = false;
            DateTime = DateTime.Now;
        }
    }
}
