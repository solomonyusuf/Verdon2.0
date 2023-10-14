using System.ComponentModel.DataAnnotations;

namespace XeerLearn.Models.Utility
{
    #nullable disable
    public class Meeting
    {
        [Key]
        public Guid Id { get; set; }

        public Guid AccessKeyId { get; set; }

        public string XeerLearnUserId { get; set; }

        [Required]
        public string Name { get; set; }
        
        public string ModeratorPwd { get; set; }

        public string AttendeePwd { get; set; }

        public string LogoutUrl { get; set; }

        public string Logo { get; set; }

        public string Background { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime DateTime { get; set; }

        public Meeting()
        {
            DateTime = DateTime.Now;  
            AttendeePwd = new Random().Next().ToString();  
            ModeratorPwd = new Random().Next().ToString();
            LogoutUrl = "https://localhost:7228/check-return";
        }


    }
}
