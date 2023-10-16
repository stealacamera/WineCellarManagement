using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WineCellar.DataAccess;
using WineCellar.DataAccess.Repositories.IRepositories;
using WineCellar.Models;

namespace WineCellar.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IWorkUnit _workUnit;

        public RoleController(IWorkUnit workUnit, RoleManager<AppRole> roleManager)
        {
            _workUnit = workUnit;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View(_roleManager.Roles.ToList());
        }

        #region API
        [HttpPost]
        public IActionResult Upsert([FromBody] AppRole instance)
        {
            if(ModelState.IsValid)
            {                
                if(_roleManager.FindByIdAsync(instance.Id).GetAwaiter().GetResult() == null)
                {
                    _roleManager.CreateAsync(instance).GetAwaiter().GetResult();
                    return CreatedAtAction(null, new { id = instance.Id, name = instance.Name });
                }
                else
                {
                    _roleManager.UpdateAsync(instance).GetAwaiter().GetResult();
                    return Ok(instance);
                }
            }

            return BadRequest(new { message = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage)).First() });
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] string[] ids)
        {
            if (ids.Length == 0)
                return Ok();

            List<AppRole> instances = new List<AppRole>();

            foreach(string id in ids)
            {
                AppRole instance = _roleManager.FindByIdAsync(id).GetAwaiter().GetResult();

                if (instance == null)
                    return NotFound(new {message = "Something went wrong: Entity could not be found"});

                instances.Add(instance);
            }

            _workUnit.AppRole.RemoveRange(instances);
            _workUnit.Save();

            return Ok();
        }
        #endregion
    }
}
