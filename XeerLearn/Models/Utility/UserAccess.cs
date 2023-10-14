using System.ComponentModel.DataAnnotations;

namespace XeerLearn.Models.Utility
{
    public class UserAccess
    {
        [Key]
        public Guid Id { get; set; }
        public Guid AccessKeyId { get; set; }
        public string Type { get; set; }
        public string XeerLearnUserId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime DateTime { get; set; }


        public UserAccess()
        {
            DateTime = DateTime.Now;
        }
    }
}
