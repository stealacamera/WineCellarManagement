using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WineCellar.Models.ViewModels
{
    public class ProductionEntitiesVM
    {
        public PaginatedResponse<Varietal> Varietals { get; set; } = null!;
        public PaginatedResponse<WineProducer> WineProducers { get; set; } = null!;
    }
}
