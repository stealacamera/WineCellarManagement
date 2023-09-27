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
        private readonly IWorkUnit db;

        public CountryController(IWorkUnit db)
        {
            this.db = db;
        }

        public IActionResult Index(int page = 1)
        {
            int pageSize = 8;

            PaginatedResponse<Country> result = new()
            {
                Entities = db.Country.GetAll(
                    orderBy: x => x.Id,
                    pageSize: pageSize, pageNumber: page
                //includeProps: "Regions"
                ),
                Pagination = new Pagination(db.Country.Count(), page, pageSize)
            };

            return View(result);
        }

        #region API
        [HttpGet]
        public IActionResult GetRegions(int id)
        {
            Country? instance = db.Country.GetFirstOrDefault(x => x.Id == id, "Regions");

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
                    db.Country.Add(instance);
                    db.Save();
                    return CreatedAtAction(null, new { id = instance.Id, name = instance.Name });
                }
                else
                {
                    db.Country.Update(instance);
                    db.Save();
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

            Country instance = db.Country.GetFirstOrDefault(x => x.Id == id)!;

            if (instance == null)
                return NotFound(new { message = "Something went wrong: Item could not be found. Try again later." });

            db.Country.Remove(instance);
            db.Save();
            return NoContent();
        }
        #endregion
    }
}
