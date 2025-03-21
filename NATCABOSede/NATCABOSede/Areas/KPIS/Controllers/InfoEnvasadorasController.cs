using Microsoft.AspNetCore.Mvc;
using NATCABOSede.Models;
using NATCABOSede.ViewModels;

namespace NATCABOSede.Areas.KPIS.Controllers
{
    [Area("KPIS")]
    public class InfoEnvasadorasController : Controller
    {
        private readonly NATCABOContext _context;

        public InfoEnvasadorasController(NATCABOContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var datosMock = new DatosKpiViewModel
            {
                NombreLinea = "Línea Mock",
                Cliente = "Cliente de prueba",
                Producto = "Producto X",
                PesoTotalDesperdicio = 150.75,
                PorcentajeTotalDesperdicio = 12.5,
                FTT = 89.7
            };

            return View(datosMock);
        }
    }
}
