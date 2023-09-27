using Microsoft.AspNetCore.Mvc;

namespace WineCellar.Controllers
{
    public class ViewComponentController : Controller
    {
        [HttpPost]
        public IActionResult CreateCmsRow(int id, string name, string? idPrefix = null)
        {
            return ViewComponent("CmsRow", new { id, name, idPrefix });
        }
    }
}
