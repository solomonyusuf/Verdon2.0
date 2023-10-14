using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace XeerLearn.Models.Auth
{
    [NotMapped]
    public class XeerLearnUserRole:IdentityUserRole<string>
    {
    }
}
