using ERP.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace ERP.Models
{
    public class User : IdentityUser
    {
        public virtual ICollection<Business>? Businesses { get; set; }
        public User() : base() { }
    }
}
