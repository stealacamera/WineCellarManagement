using Microsoft.AspNetCore.Identity;

namespace WineCellar.Models
{
    public class AppUserRole : IdentityUserRole<string>
    {
        public virtual AppUser User { get; set; } = null!;
        public virtual AppRole Role { get; set; } = null!;
    }
}
