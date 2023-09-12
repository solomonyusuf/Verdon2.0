using Microsoft.AspNetCore.Identity;
using System.Collections;
using Verdon.Models.Money;


namespace Verdon.Models.Auth
{
    #nullable disable
    public class VerdonUser : IdentityUser
    {
        public virtual string Image { get; set; }
        public virtual string CoverImage { get; set; }
        public virtual string Initials { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string MiddleName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Description { get; set; }
        public virtual string Matric { get; set; }
        public virtual string Type { get; set; }
        public virtual string Address { get; set; }
        public virtual string Country { get; set; }
        public virtual string State { get; set; }
        public virtual string Currency { get; set; }
        public virtual int BankCode { get; set; }
        public virtual long AccountNumber { get; set; }
        public virtual string AccountName { get; set; }
        public virtual string BankName { get; set; }
        public virtual DateTime DateTime { get; set; }


        public VerdonUser()
        {
            DateTime = DateTime.Now;
        }

    }
}
