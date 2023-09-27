using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace WineCellar.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(45)]
        public string Name { get; set; } = null!;

        [ValidateNever]
        public virtual ICollection<Region>? Regions { get; private set; }
    }
}
