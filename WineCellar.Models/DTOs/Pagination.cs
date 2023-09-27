using WineCellar.Utilities;

namespace WineCellar.Models.DTOs
{
    public class Pagination
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public string Name { get; set; }

        public Pagination(int totalItems, int currPage, int pageSize = Consts.PAGINATION_SIZE, string name = "page")
        {
            TotalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);

            TotalItems = totalItems;
            CurrentPage = currPage;
            PageSize = pageSize;

            Name = name;
        }
    }
}
