using System.ComponentModel.DataAnnotations;

namespace XeerLearn.Models.Utility
{
    #nullable disable
    public class OrderHistory
    {
        [Key]
        public Guid Id { get; set; }
        public Guid AccessKeyId { get; set; }
        public Guid OrderId { get; set; }
        public string XeerLearnUserId { get; set; }
        public DateTime Expiry { get; set; }
        public DateTime DateTime { get; set; }


        public OrderHistory()
        {
            Id = Guid.Parse(Guid.NewGuid().ToString("N"));
            DateTime = DateTime.Now;
        }
    }
}
