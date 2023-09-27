using System.ComponentModel.DataAnnotations;

namespace WineCellar.Models
{
    public class Varietal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        public string Name { get; set; } = null!;
    }
}