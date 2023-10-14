namespace XeerLearn.Models.Money
{
    #nullable disable
    public class Wallet
    {
        public virtual Guid Id { get; set; }
        public Guid AccessKeyId { get; set; }
        public virtual string XeerLearnUserId { get; set; }
        public virtual double Balance { get; set; }
        public virtual DateTime DateTime { get; set; }

        public Wallet()
        {
            DateTime = DateTime.Now;
        }
    }
}
