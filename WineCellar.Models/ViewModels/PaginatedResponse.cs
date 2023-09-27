using WineCellar.Models.DTOs;

namespace WineCellar.Models.ViewModels
{
    public class PaginatedResponse<T> where T : class
    {
        public IEnumerable<T> Entities { get; set; } = null!;
        public Pagination Pagination { get; set; } = null!;
    }
}