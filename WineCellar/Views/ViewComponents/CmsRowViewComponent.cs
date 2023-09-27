using Microsoft.AspNetCore.Mvc;

namespace WineCellar.Views.ViewComponents
{
    public class CmsRowViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int id, string name)
        {
            return View(new { Id = id, Name = name });
        }
    }
}