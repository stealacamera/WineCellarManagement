using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WineCellar.Models
{
    public class AppUser : IdentityUser
    {
        public int? EstablishmentId { get; set; }

        [ValidateNever]
        public Establishment? Establishment { get; set; }

        [ValidateNever]
        public virtual ICollection<AppRole> Roles { get; private set; } = null!;
    }
}
