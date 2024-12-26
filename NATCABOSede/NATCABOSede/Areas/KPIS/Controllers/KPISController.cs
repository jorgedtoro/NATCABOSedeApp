using Microsoft.AspNetCore.Mvc;
using NATCABOSede.Models;
using NATCABOSede.Services;
using NATCABOSede.ViewModels;
using NATCABOSede.Interfaces;

namespace NATCABOSede.Areas.KPIS.Controllers
{
    [Area("KPIS")]
    public class KPISController : Controller
    {
        private readonly NATCABOContext _context;
        private readonly IKPIService _kpiService;

        public KPISController(NATCABOContext context, IKPIService kpiService)
        {
            _context = context;
            _kpiService = kpiService;
        }

        public IActionResult Index(short lineaSeleccionada = 0) // Default to 0, indicating no line selected yet
        {
            // Fetch available lines dynamically
            var lineas = _context.DatosKpis
                .Select(d => new { d.IdLinea, d.NombreLinea })
                .ToList();

            // Pass lines to the view using ViewBag
            ViewBag.LineasDisponibles = lineas;

            // If no line is selected, pick the first one
            if (lineaSeleccionada == 0 && lineas.Any())
            {
                lineaSeleccionada = lineas.First().IdLinea; // Set to the first available line's Id
            }

            // Set the selected line in ViewBag
            ViewBag.LineaSeleccionada = lineaSeleccionada;

            // Find the name of the selected line
            var lineaSeleccionadaNombre = lineas.FirstOrDefault(l => l.IdLinea == lineaSeleccionada)?.NombreLinea;
            ViewBag.NombreLineaSeleccionada = lineaSeleccionadaNombre ?? "Nombre no disponible";

            // Fetch KPI data for the selected line
            var datos = ObtenerDatosPorLinea(lineaSeleccionada);
            double mediaPaquetesPorMinuto = 0.0;

            if (datos == null)
            {
                return NotFound("No se encontraron datos para la línea seleccionada.");
            }

            if (datos.HoraInicioProduccion != null && datos.HoraUltimoPaquete != null)
            {
                var inicio = datos.HoraInicioProduccion.Value;
                var fin = datos.HoraUltimoPaquete.Value;

                // Calculate time difference
                TimeSpan diferencia = fin - inicio;
                mediaPaquetesPorMinuto = datos.PaquetesValidos / diferencia.TotalMinutes;
            }

            var ppm = _kpiService.CalcularPPM(
                datos.PaquetesValidos,
                datos.MinutosTrabajados ?? 0,
                datos.NumeroOperadores ?? 0);

            var pm = _kpiService.CalcularPM(
                datos.PaquetesValidos,
                datos.MinutosTrabajados ?? 0);

            var extraPeso = _kpiService.CalcularExtrapeso(
                datos.PesoTotalReal ?? 0,
                datos.PesoObjetivo ?? 0,
                datos.PaquetesValidos);

            var horaFinAproximada = _kpiService.CalcularHoraFin(
                datos.HoraInicioProduccion ?? DateTime.Now,
                (int)(datos.PaquetesRequeridos - datos.PaquetesValidos),
                mediaPaquetesPorMinuto);

            var porcentajePedido = _kpiService.CalcularPorcentajePedido(
                (int)(datos.PaquetesValidos),
                (int)(datos.PaquetesRequeridos));

            var costeMOD = _kpiService.CalcularCosteMOD(
                datos.MinutosTrabajados ?? 0,
                datos.CosteHora ?? 0,
                datos.PaquetesValidos,
                datos.PesoObjetivo ?? 0);

            var modelo = new DatosKpiViewModel
            {
                Cliente = datos.NombreCliente ?? "*CLIENTE*",
                Producto = datos.NombreProducto ?? "*PRODUCTO*",
                PPM = ppm,
                PM = pm,
                ExtraPeso = extraPeso,
                HoraInicio = datos.HoraInicioProduccion ?? DateTime.Now,
                HoraFinAproximada = horaFinAproximada,
                PorcentajePedido = porcentajePedido,
                CosteMOD = costeMOD
            };

            return View(modelo);
        }


        public IActionResult ObtenerKPIs(short lineaSeleccionada)
        {
            var datos = ObtenerDatosPorLinea(lineaSeleccionada);

            if (datos == null)
            {
                return NotFound("No se encontraron datos para la línea seleccionada.");
            }

            double mediaPaquetesPorMinuto = 0.0;
            if (datos.HoraInicioProduccion != null && datos.HoraUltimoPaquete != null)
            {
                var inicio = datos.HoraInicioProduccion.Value;
                var fin = datos.HoraUltimoPaquete.Value;

                // Calculate time difference
                TimeSpan diferencia = fin - inicio;
                mediaPaquetesPorMinuto = datos.PaquetesValidos / diferencia.TotalMinutes;
            }

            var ppm = _kpiService.CalcularPPM(
                datos.PaquetesValidos,
                datos.MinutosTrabajados ?? 0,
                datos.NumeroOperadores ?? 0);

            var pm = _kpiService.CalcularPM(
                datos.PaquetesValidos,
                datos.MinutosTrabajados ?? 0);

            var extraPeso = _kpiService.CalcularExtrapeso(
                datos.PesoTotalReal ?? 0,
                datos.PesoObjetivo ?? 0,
                datos.PaquetesValidos);

            var horaFinAproximada = _kpiService.CalcularHoraFin(
                datos.HoraInicioProduccion ?? DateTime.Now,
                (int)(datos.PaquetesRequeridos - datos.PaquetesValidos),
                mediaPaquetesPorMinuto);

            var porcentajePedido = _kpiService.CalcularPorcentajePedido(
                (int)(datos.PaquetesValidos),
                (int)(datos.PaquetesRequeridos));

            var costeMOD = _kpiService.CalcularCosteMOD(
                datos.MinutosTrabajados ?? 0,
                datos.CosteHora ?? 0,
                datos.PaquetesValidos,
                datos.PesoObjetivo ?? 0);

            var modelo = new DatosKpiViewModel
            {
                Cliente = datos.NombreCliente ?? "*CLIENTE*",
                Producto = datos.NombreProducto ?? "*PRODUCTO*",
                PPM = ppm,
                PM = pm,
                ExtraPeso = extraPeso,
                HoraInicio = datos.HoraInicioProduccion ?? DateTime.Now,
                HoraFinAproximada = horaFinAproximada,
                PorcentajePedido = porcentajePedido,
                CosteMOD = costeMOD
            };

            return PartialView("_KPIsPartial", modelo);
        }

        private DatosKpi ObtenerDatosPorLinea(short linea)
        {
            try
            {
            return _context.DatosKpis.FirstOrDefault(d => d.IdLinea == linea);

            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        // Recuperación de líneas activas desde la bbdd
        [HttpGet]
        public JsonResult ObtenerLineas()
        {
            var lineas = _context.DatosKpis
                .Select(d => new { d.IdLinea, d.NombreLinea })
                .ToList();

            if (!lineas.Any())
            {
                Console.WriteLine("No data found in DatosKpis table");
            }

            return Json(lineas);
        }


    }


}

