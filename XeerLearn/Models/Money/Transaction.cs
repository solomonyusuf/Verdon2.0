namespace XeerLearn.Models.Money
{
    #nullable disable
    public class Transaction
    {
        public virtual Guid Id { get; set; }
        public virtual Guid OrderId { get; set; }
        public virtual Guid AccessKeyId { get; set; }
        public virtual string XeerLearnUserId { get; set; }
        public virtual string AccountName { get; set; }
        public virtual string AccountNumber { get; set; }
        public virtual string Currency { get; set; }
        public virtual double Amount { get; set; }
        public virtual double Balance { get; set; }
        public virtual DateTime ExpiryDate { get; set; }
        public virtual DateTime DateTime { get; set; }

        public Transaction()
        {
            DateTime = DateTime.Now;
        }
    }
}
