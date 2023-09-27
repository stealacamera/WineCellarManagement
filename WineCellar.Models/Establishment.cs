using System.ComponentModel.DataAnnotations;

namespace WineCellar.Models
{
    public class Establishment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;
    }
}
