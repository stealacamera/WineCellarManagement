using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WineCellar.Utilities;

namespace WineCellar.Models.ViewModels
{
    public class EmployeeUserVM
    {
        public EmployeeUserVM() { }

        public EmployeeUserVM(RoleManager<AppRole> roleManager)
        {
            RolesList = roleManager.Roles.Where(x => x.Name != Consts.Role_Admin).ToList()
                    .Select(y => new SelectListItem { Text = y.Name, Value = y.Name });

            Role = roleManager.FindByNameAsync(Consts.Role_Employee).GetAwaiter().GetResult();
        }

        public string? Id { get; set; } = null;

        [Required]
        [StringLength(255)]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public AppRole Role { get; set; } = null!;

        [ValidateNever]
        public IEnumerable<SelectListItem> RolesList { get; set; } = null!;

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
