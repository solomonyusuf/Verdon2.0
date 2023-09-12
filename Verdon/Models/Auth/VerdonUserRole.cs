using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Verdon.Models.Auth
{
    [NotMapped]
    public class VerdonUserRole:IdentityUserRole<string>
    {
    }
}
