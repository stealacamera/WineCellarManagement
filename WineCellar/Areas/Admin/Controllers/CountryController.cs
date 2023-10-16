using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using WineCellar.DataAccess.Repositories.IRepositories;
using WineCellar.Models;
using WineCellar.Models.DTOs;
using WineCellar.Models.ViewModels;

namespace WineCellar.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CountryController : Controller
    {
        private readonly IWorkUnit _workUnit;

        public CountryController(IWorkUnit workUnit)
        {
            _workUnit = workUnit;
        }

        public IActionResult Index(int page = 1)
        {
            int pageSize = 8;

            PaginatedResponse<Country> result = new()
            {
                Entities = _workUnit.Country.GetAll(
                    orderBy: x => x.Id,
                    pageSize: pageSize, pageNumber: page
                //includeProps: "Regions"
                ),
                Pagination = new Pagination(_workUnit.Country.Count(), page, pageSize)
            };

            return View(result);
        }

        #region API
        [HttpGet]
        public IActionResult GetRegions(int id)
        {
            Country? instance = _workUnit.Country.GetFirstOrDefault(x => x.Id == id, "Regions");

            if (instance == null)
                return NotFound(new { message = "ID not valid: Country not found" });

            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };

            var result = Json(instance.Regions, options);
            result.StatusCode = StatusCodes.Status200OK;

            return result;
        }

        [HttpPost]
        public IActionResult Upsert([FromBody] Country instance)
        {
            if (ModelState.IsValid)
            {
                if (instance.Id == 0)
                {
                    _workUnit.Country.Add(instance);
                    _workUnit.Save();
                    return CreatedAtAction(null, new { id = instance.Id, name = instance.Name });
                }
                else
                {
                    _workUnit.Country.Update(instance);
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

            Country instance = _workUnit.Country.GetFirstOrDefault(x => x.Id == id)!;

            if (instance == null)
                return NotFound(new { message = "Something went wrong: Item could not be found. Try again later." });

            _workUnit.Country.Remove(instance);
            _workUnit.Save();
            return NoContent();
        }
        #endregion
    }
}
