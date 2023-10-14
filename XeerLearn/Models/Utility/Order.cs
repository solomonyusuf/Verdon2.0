using System.ComponentModel.DataAnnotations;

namespace XeerLearn.Models.Utility
{
    #nullable disable
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public int Students { get; set; }
        public int Tutors { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime DateTime { get; set; }


        public Order()
        {
            Id = Guid.Parse(Guid.NewGuid().ToString("N"));
            DateTime = DateTime.Now;
        }
    }
}
