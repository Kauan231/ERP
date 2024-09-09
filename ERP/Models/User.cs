using Microsoft.AspNetCore.Identity;

namespace ERP.Models
{
    public class User : IdentityUser
    {
        public virtual ICollection<Inventory>? Inventories { get; set; }
        public User() : base() { }
    }
}
