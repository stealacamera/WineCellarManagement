using System.ComponentModel.DataAnnotations;

namespace WineCellar.Models
{
    public class WineProducer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(35)]
        public string Name { get; set; } = null!;
    }
}
