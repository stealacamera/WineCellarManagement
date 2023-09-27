using Microsoft.AspNetCore.Mvc;
using WineCellar.Models.DTOs;

namespace WineCellar.Views.ViewComponents
{
    public class PaginationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Pagination pagination)
        {
            return View(pagination);
        }
    }
}
