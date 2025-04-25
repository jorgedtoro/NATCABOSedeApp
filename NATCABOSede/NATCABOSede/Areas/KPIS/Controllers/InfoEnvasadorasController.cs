using ClosedXML.Graphics;
using DocumentFormat.OpenXml.Spreadsheet;
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
            var datosPre = (from kpi in _context.DatosKpisLives
                          join marco in _context.TMarcoBizerbas on kpi.IdLinea equals marco.IdLineaMarco
                          where marco.DeviceNoBizerba == discriminadora
                          select new LineaKpiViewModel
                          {
                              NombreLinea = kpi.NombreLinea,
                              PPM = kpi.PpmPersonalEnBalanza ?? 0,
                              Objetivo = kpi.PpmObjPersonaEnBalanza,
                              NombreCliente=kpi.NombreCliente ?? "",
                              NombreProducto = kpi.NombreProducto ?? ""
                            
                           }).ToList();
            var lineas = datosPre.Select(d => new LineaKpiViewModel
            {
                NombreLinea = d.NombreLinea,
                PPM = d.PPM,
                Objetivo = d.Objetivo,
                PpmCardClass = GetColorClass(d.PPM, d.Objetivo),
                NombreCliente = d.NombreCliente ?? "",
                NombreProducto = d.NombreProducto ?? ""
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
        private string GetColorClass(double actual, double objetivo)
        {
            double porcentaje = ((actual - objetivo) / objetivo) * 100;

            if (porcentaje > 10)
                return "bg-success";  // Verde
            else if (porcentaje >= -10 && porcentaje <= 10)
                return "bg-warning";  // Amarillo
            else
                return "bg-danger";   // Rojo
        }
    }
}