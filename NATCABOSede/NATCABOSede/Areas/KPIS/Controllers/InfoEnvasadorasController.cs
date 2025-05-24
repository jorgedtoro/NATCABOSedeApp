using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NATCABOSede.Models;
using NATCABOSede.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NATCABOSede.Areas.KPIS.Controllers
{
    [Area("KPIS")]
    [Route("KPIS/[controller]/[action]")]
    public class InfoEnvasadorasController : Controller
    {
        private readonly NATCABOContext _context;
        private readonly ILogger<InfoEnvasadorasController> _logger;

        public InfoEnvasadorasController(NATCABOContext context, ILogger<InfoEnvasadorasController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> Index(int discriminadora)
        {
            try
            {
                _logger.LogInformation($"Solicitando datos para la discriminadora: {discriminadora}");

                var datosPre = await (from kpi in _context.DatosKpisLives
                                    join marco in _context.TMarcoBizerbas 
                                        on kpi.IdLinea equals marco.IdLineaMarco
                                    where marco.DeviceNoBizerba == discriminadora && kpi.IdLinea.HasValue
                                    select new LineaKpiViewModel
                                    {
                                        NombreLinea = kpi.NombreLinea ?? "Sin nombre",
                                        PPM = kpi.PpmPersonalEnBalanza ?? 0,
                                        Objetivo = kpi.PpmObjPersonaEnBalanza,
                                        NombreCliente = kpi.NombreCliente ?? "Sin cliente",
                                        NombreProducto = kpi.NombreProducto ?? "Sin producto"
                                    })
                                    .AsNoTracking()
                                    .ToListAsync();

                var lineas = datosPre.Select(d => new LineaKpiViewModel
                {
                    NombreLinea = d.NombreLinea,
                    PPM = d.PPM,
                    Objetivo = d.Objetivo,
                    PpmCardClass = GetColorClass(d.PPM, d.Objetivo),
                    NombreCliente = d.NombreCliente,
                    NombreProducto = d.NombreProducto
                }).ToList();

                ViewBag.Discriminadora = discriminadora;
                _logger.LogInformation($"Se encontraron {lineas.Count} líneas para la discriminadora {discriminadora}");

                return View(lineas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener datos para la discriminadora {discriminadora}");
                // Podrías redirigir a una página de error o retornar una vista de error
                return StatusCode(500, "Ocurrió un error al procesar su solicitud. Por favor, intente nuevamente más tarde.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerDiscriminadoras()
        {
            try
            {
                _logger.LogInformation("Solicitando lista de discriminadoras");
                
                var discriminadoras = await _context.TMarcoBizerbas
                    .Where(x => x.DeviceNoBizerba > 0)
                    .Select(x => x.DeviceNoBizerba)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToListAsync();

                _logger.LogInformation($"Se encontraron {discriminadoras.Count} discriminadoras");
                return Json(discriminadoras);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de discriminadoras");
                return StatusCode(500, "Error al obtener la lista de discriminadoras");
            }
        }

        private string GetColorClass(double actual, double objetivo)
        {
            if (objetivo <= 0) return "bg-secondary"; // Gris si no hay objetivo definido

            double porcentaje = ((actual - objetivo) / objetivo) * 100;

            if (porcentaje > 10)
                return "bg-success";  // Verde
            else if (porcentaje >= -10)
                return "bg-warning";  // Amarillo
            else
                return "bg-danger";   // Rojo
        }
    }
}