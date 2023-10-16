using Microsoft.AspNetCore.Mvc;
using WineCellar.DataAccess.Repositories.IRepositories;
using WineCellar.Models.DTOs;
using WineCellar.Models.ViewModels;
using WineCellar.Models;

namespace WineCellar.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductionEntitiesController : Controller
    {
        private readonly IWorkUnit _workUnit;

        public ProductionEntitiesController(IWorkUnit workUnit)
        {
            _workUnit = workUnit;
        }

        public IActionResult Index(int varietalPage = 1, int producerPage = 1)
        {
            int varietalPageSize = 15;

            ProductionEntitiesVM result = new()
            {
                Varietals = new PaginatedResponse<Varietal>
                {
                    Entities = _workUnit.Varietal.GetAll(
                        orderBy: x => x.Id,
                        pageSize: varietalPageSize,
                        pageNumber: varietalPage
                        ),
                    Pagination = new Pagination(_workUnit.Varietal.Count(), varietalPage, varietalPageSize, name: "varietalPage")
                },
                WineProducers = new PaginatedResponse<WineProducer>
                {
                    Entities = _workUnit.WineProducer.GetAll(
                        orderBy: x => x.Id,
                        pageNumber: producerPage
                        ),
                    Pagination = new Pagination(_workUnit.WineProducer.Count(), producerPage, name: "producerPage")
                }
            };

            return View(result);
        }
    }
}
