using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WineCellar.Models
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base()
        {
        }

        public AppRole(string roleName) : base(roleName)
        {
        }

        [ValidateNever]
        public virtual ICollection<AppUser>? Users { get; private set; }
    }
}
