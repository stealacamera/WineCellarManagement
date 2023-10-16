using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using WineCellar.DataAccess.Repositories.IRepositories;
using WineCellar.Models;
using WineCellar.Utilities;
using WineCellar.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace WineCellar.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = Consts.Role_Manager)]
    public class WorkForceController : Controller
    {
        private const string SessionRolesList = "RolesList";

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IWorkUnit _workUnit;

        public WorkForceController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IWorkUnit workUnit)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _workUnit = workUnit;
        }

        public IActionResult Index(int page = 1)
        {
            var currUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            AppUser currUser = _workUnit.AppUser.GetFirst(x => x.Id == currUserId);

            // Gets all company roles except manager
            List<AppRole> lowLvlRoles = _roleManager.Roles.Where(x => x.Name != Consts.Role_Admin).ToList();

            // Gets all employees under the user's establishment
            IEnumerable <AppUser> workers = _workUnit.AppUser.GetAll(
                x => x.Id != currUser.Id && x.EstablishmentId == currUser.EstablishmentId && lowLvlRoles.Contains(x.Roles.First()),
                pageNumber: page,
                includeProps: "Roles"
            );

            PaginatedResponse<AppUser> result = new PaginatedResponse<AppUser> 
            { 
                Entities = workers, 
                Pagination = new Models.DTOs.Pagination(workers.Count(), page) 
            };

            return View(result);
        }

        public IActionResult Upsert(string? id)
        {
            EmployeeUserVM model = new EmployeeUserVM(_roleManager);
            AppUser? instanceUser = null;

            if (!id.IsNullOrEmpty())
                instanceUser = _workUnit.AppUser.GetFirstOrDefault(x => x.Id == id, includeProps: "Roles");

            // Sets up employee view model if new user is being created
            // or existing user is being edited
            if (instanceUser == null)
            {
                var currUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                AppUser currUser = _workUnit.AppUser.GetFirst(x => x.Id == currUserId, includeProps: "Establishment");

                model.Password = $"{currUser.Establishment!.Name}_123"; // Base password for establishment's employees
            }
            else
            {
                model.Id = instanceUser.Id;
                model.Username = instanceUser.UserName;
                model.Email = instanceUser.Email;
                model.Role = instanceUser.Roles.First();
            }
            
            HttpContext.Session.SetString(SessionRolesList, JsonConvert.SerializeObject(model.RolesList));

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(EmployeeUserVM instance)
        {
            if(!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionRolesList)))
                instance.RolesList = JsonConvert.DeserializeObject<IEnumerable<SelectListItem>>(HttpContext.Session.GetString(SessionRolesList)!)!;

            if (ModelState.IsValid)
            {
                if (instance.Id.IsNullOrEmpty())
                {
                    var currUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    AppUser currUser = _workUnit.AppUser.GetFirst(x => x.Id == currUserId, includeProps: "Establishment");

                    AppUser newUser = new AppUser {
                        UserName = instance.Username,
                        Email = instance.Email,
                        EstablishmentId = currUser.EstablishmentId,
                        EmailConfirmed = true 
                    };
                    
                    _userManager.CreateAsync(newUser, instance.Password).GetAwaiter().GetResult();
                    var result = _userManager.AddToRoleAsync(newUser, instance.Role.Name).GetAwaiter().GetResult();

                    // If user entity is not created, show errors
                    if(!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);

                        return View(instance);
                    } else
                        TempData["success"] = $"{instance.Email} was successfully added";
                }
                else
                {
                    AppUser? updatedUser = _workUnit.AppUser.GetFirstOrDefault(x => x.Id == instance.Id, includeProps: "Roles");

                    // Return page with error if user is not found
                    if (updatedUser == null)
                    {
                        TempData["failure"] = $"Something went wrong: Please try again later";
                        return View(instance);
                    }
                    else
                    {
                        updatedUser.UserName = instance.Username;
                        updatedUser.Email = instance.Email;
                        _userManager.UpdateAsync(updatedUser).GetAwaiter().GetResult();
                        
                        if(!updatedUser.Roles.First().Name.Equals(instance.Role.Name))
                        {
                            _userManager.RemoveFromRoleAsync(updatedUser, updatedUser.Roles.First().Name).GetAwaiter().GetResult();
                            _userManager.AddToRoleAsync(updatedUser, instance.Role.Name).GetAwaiter().GetResult();
                        }

                        TempData["success"] = $"{instance.Email} was successfully updated";
                    }
                }

                return RedirectToAction("Index");
            }

            return View(instance);
        }

        #region API
        [HttpDelete]
        public IActionResult Delete(string? id)
        {
            if (id.IsNullOrEmpty())
                return BadRequest(new { message = "ID is required" });

            AppUser? instance = _workUnit.AppUser.GetFirstOrDefault(x => x.Id == id);

            if (instance == null)
                return NotFound(new { message = "Entity could not be found; check ID is correct" });
            
            if (_userManager.DeleteAsync(instance).GetAwaiter().GetResult().Succeeded)
                return NoContent();
            else
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Something went wrong: Try again later. If problem persists, contact us" });
        }
        #endregion
    }
}


/*
 * emp profile page - username, password
 * man profile page - username, password, establishment name, delete account (+establishment)
 * 
 * emp from man pov - username, email, position, abilities
 */