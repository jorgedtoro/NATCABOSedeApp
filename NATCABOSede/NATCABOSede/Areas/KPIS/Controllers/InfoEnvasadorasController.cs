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
        public IActionResult Index(int discriminadora)
        {
            var lineas = (from kpi in _context.DatosKpisLives
                          join marco in _context.TMarcoBizerbas on kpi.IdLinea equals marco.IdLineaMarco
                          where marco.DeviceNoBizerba == discriminadora
                          select new LineaKpiViewModel
                          {
                              NombreLinea = kpi.NombreLinea,
                              PPM = kpi.PpmBizerba ?? 0,
                              Objetivo = kpi.PpmObjetivo ?? 0
                          }).ToList();

            ViewBag.Discriminadora = discriminadora;
            return View(lineas);
        }
        [HttpGet]
        public IActionResult ObtenerDiscriminadoras()
        {
            var discriminadoras = _context.TMarcoBizerbas
                .Where(x => x.DeviceNoBizerba > 0)
                .Select(x => x.DeviceNoBizerba)
                .Distinct()
                .ToList();

            return Json(discriminadoras);
        }

    }
}