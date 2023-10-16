using Microsoft.AspNetCore.Mvc;
using WineCellar.DataAccess.Repositories.IRepositories;
using WineCellar.Models;

namespace WineCellar.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WineProducerController : Controller
    {
        private readonly IWorkUnit _workUnit;

        public WineProducerController(IWorkUnit workUnit)
        {
            _workUnit = workUnit;
        }

        #region API
        [HttpPost]
        public IActionResult Upsert([FromBody] WineProducer instance)
        {
            if (ModelState.IsValid)
            {
                if (instance.Id == 0)
                {
                    _workUnit.WineProducer.Add(instance);
                    _workUnit.Save();
                    return CreatedAtAction(null, new { id = instance.Id, name = instance.Name });
                }
                else
                {
                    _workUnit.WineProducer.Update(instance);
                    _workUnit.Save();
                    return Ok();
                }
            }
            else
                return BadRequest(new { message = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage)).First() });

        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return BadRequest(new { message = "Something went wrong. Try again later." });

            WineProducer instance = _workUnit.WineProducer.GetFirstOrDefault(x => x.Id == id)!;

            if (instance == null)
                return NotFound(new { message = "Something went wrong: Item could not be found. Try again later." });

            _workUnit.WineProducer.Remove(instance);
            _workUnit.Save();
            return NoContent();
        }
        #endregion
    }
}
