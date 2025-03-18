using Microsoft.AspNetCore.Mvc;
using NATCABOSede.Models;
using NATCABOSede.ViewModels;

namespace NATCABOSede.Areas.KPIS.Controllers
{
    [Area("KPIS")]
    public class InfoDiscriminadoraController : Controller
    {
        private readonly NATCABOContext _context;

        public InfoDiscriminadoraController(NATCABOContext context)
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
                PPM = 100,
                PM = 90,
                ExtraPeso = 5.5,
                HoraInicio = DateTime.Now.AddHours(-2),
                HoraFinAproximada = DateTime.Now.AddHours(1),
                PorcentajePedido = 75,
                CosteMOD = 12.5
            };

            return View(datosMock);
        }
    }
}
