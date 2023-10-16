using Microsoft.AspNetCore.Mvc;
using WineCellar.DataAccess.Repositories.IRepositories;
using WineCellar.Models;

namespace WineCellar.Areas.Manager.Controllers
{
    public class EstablishmentController : Controller
    {
        private readonly IWorkUnit _workUnit;

        public EstablishmentController(IWorkUnit workUnit)
        {
            _workUnit = workUnit;
        }

        #region API
        [HttpPost]
        public IActionResult Upsert([FromBody] Establishment instance)
        {
            if (ModelState.IsValid)
            {
                if (instance.Id == 0)
                {
                    _workUnit.Establishment.Add(instance);
                    _workUnit.Save();
                    return CreatedAtAction(null, null /*TODO dont forget to remove if not needed => new { id = instance.Id, name = instance.Name }*/);
                }
                else
                {
                    _workUnit.Establishment.Update(instance);
                    _workUnit.Save();
                    return Ok();
                }
            }
            else
                return BadRequest(new { message = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage)) });
        }
        #endregion
    }
}
