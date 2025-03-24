// Controllers/KPISController.cs
using Microsoft.AspNetCore.Mvc;
using NATCABOSede.Models;
using NATCABOSede.Services;
using NATCABOSede.ViewModels;
using NATCABOSede.Interfaces;
using NATCABOSede.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        public IActionResult Index(short lineaSeleccionada = 0) // Default to 0, indicating no line selected yet
        {
            var datos = ObtenerDatosPorLinea(lineaSeleccionada);
            DatosKpiViewModel modelo;

            // Fetch available lines dynamically
            var lineas = _context.DatosKpisLives
                .Select(d => new { d.IdLinea, d.NombreLinea })
                .Distinct()
                .ToList();

            // Pass lines to the view using ViewBag
            ViewBag.LineasDisponibles = lineas;

            // If no line is selected, pick the first one
            if (lineaSeleccionada == 0 && lineas.Any())
            {
                lineaSeleccionada = lineas.First().IdLinea ?? 0; // Set to the first available line's Id
            }

            
            ViewBag.LineaSeleccionada = lineaSeleccionada;

           
            var lineaSeleccionadaNombre = lineas.FirstOrDefault(l => l.IdLinea == lineaSeleccionada)?.NombreLinea;
            ViewBag.NombreLineaSeleccionada = lineaSeleccionadaNombre ?? "Nombre no disponible";

            if (datos == null)
            {
                // Crear un modelo vacío
                modelo = new DatosKpiViewModel
                {
                    Cliente = string.Empty,
                    Producto = string.Empty,
                    PPM = 0,
                    PPM_Disc = 0,
                    PM = 0,
                    PM_Disc=0,
                    ExtraPeso = 0,
                    HoraInicio = DateTime.Now,
                    HoraFinAproximada = DateTime.Now,
                    PorcentajePedido = 0,
                    CosteMOD = 0
                };
            }
            else
            {
                double mediaPaquetesPorMinuto = 0.0;

                if (datos.HoraInicioProduccion.HasValue && datos.HoraUltimoPaquete.HasValue)
                {
                    var inicio = datos.HoraInicioProduccion.Value;
                    var fin = datos.HoraUltimoPaquete.Value;

                    // Calculate time difference
                    TimeSpan diferencia = fin - inicio;
                    if (diferencia.TotalMinutes > 0) // Evitar división por cero
                    {
                        mediaPaquetesPorMinuto = (datos.PaquetesValidos ?? 0) / diferencia.TotalMinutes;
                    }
                }

                // Utilizar el servicio para generar el ViewModel
                modelo = _kpiService.GenerarDatosKpiViewModel(datos, mediaPaquetesPorMinuto);
            }

            return View(modelo);
        }

        public IActionResult ObtenerKPIs(short lineaSeleccionada)
        {

            var datos = ObtenerDatosPorLinea(lineaSeleccionada);

            if (datos == null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"] ?? "An unexpected error occurred."; //JMB
                // Redirect to the error page if data retrieval fails
                return RedirectToAction("Error");
            }

            double mediaPaquetesPorMinuto = 0.0;
            if (datos.HoraInicioProduccion != null && datos.HoraUltimoPaquete != null)
            {
                var inicio = datos.HoraInicioProduccion.Value;
                var fin = datos.HoraUltimoPaquete.Value;

                // Calculate time difference
                // Evitar división por cero
                double minutos = (fin - inicio).TotalMinutes;
                if (minutos > 0)
                {
                    mediaPaquetesPorMinuto = (datos.PaquetesValidos ?? 0) / minutos;
                }
           
            }

            // Generar el ViewModel utilizando el servicio
            var modelo = _kpiService.GenerarDatosKpiViewModel(datos, mediaPaquetesPorMinuto);

            // Realizar cálculos adicionales específicos para esta acción
            //modelo.PM_Disc = _kpiService.CalcularPM(datos.PaquetesTotalesDisc - datos.PaquetesRechazadosDisc, datos.MinutosTrabajados);
            modelo.PM_Disc = datos.PmBizerba ?? 0.0;
            modelo.PpmCardClass = GetColorClass(modelo.PPM, modelo.ppm_objetivo);

            // Añadir otros campos específicos
            modelo.FTT = _kpiService.CalcularFTT(datos.PaquetesTotalesDisc ?? 0, datos.PaquetesRechazadosDisc ?? 0);
            modelo.PesoTotalDesperdicio = datos.PesoTotalDesperdicio ?? 0.0;
            modelo.PorcentajeTotalDesperdicio = _kpiService.CalcularPorcentajeDesperdicio(datos.PesoTotalDesperdicio ?? 0.0, datos.PesoTotalReal ?? 0.0);
            modelo.ppm_objetivo = datos.PpmObjetivo ?? 0.0;

            return PartialView("_KPIsPartial", modelo);
        }

        private DatosKpisLive ObtenerDatosPorLinea(short linea)
        {
            try
            {
                return ExecuteWithRetry(() =>
                {
                    return _context.DatosKpisLives.FirstOrDefault(d => d.IdLinea == linea);
                });
            }
            catch (Exception ex)
            {
                Logger.LogError("Se ha producido un error al obtener los datos para los KPIs.", ex);
                throw; // Let the global exception handler deal with this
            }
        }

        // Recuperación de líneas activas desde la bbdd
        [HttpGet]
        public JsonResult ObtenerLineas()
        {
            try
            {
                var lineas = _context.DatosKpisLives
                    .Select(d => new { d.IdLinea, d.NombreLinea })
                    .Distinct()
                    .ToList();

                if (!lineas.Any())
                {
                    Console.WriteLine("No data found in DatosKpisLives table");
                }

                return Json(lineas);
            }
            catch (Exception ex)
            {
                Logger.LogError("Se ha producido un error al obtener las líneas activas.", ex);
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
                return Json(new { success = false, message = "An error occurred while processing your request." });
            }
        }

        // Método para gestionar colores MARCO - Bizerba
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

        private T ExecuteWithRetry<T>(Func<T> operation, int retryCount = 3)
        {
            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    return operation();
                }
                catch (SqlException ex) when (ex.Number == 1205) // Deadlock error number
                {
                    if (i == retryCount - 1)
                    {
                        // Log and rethrow the exception if retries are exhausted
                        Logger.LogError("Deadlock occurred. Retries exhausted.", ex);
                        throw;
                    }

                    // Wait a short time before retrying
                    Task.Delay(1000).Wait();
                }
            }
            return default;
        }

        //Código nuevo para la inclusión de las líneas activas en una misma tabla
        public IActionResult Lineas()
        {
            var kpisLineas = _context.DatosKpisLives
                .Select(d => new DatosKpiViewModel
                {
                    NombreLinea=d.NombreLinea,
                    Cliente = d.NombreCliente,
                    Producto = d.NombreProducto,
                    PPM = _kpiService.CalcularPPM(d.PaquetesValidos ?? 0, d.MinutosTrabajados ?? 0, d.NumeroOperadores ?? 0),
                    //PM = _kpiService.CalcularPM(d.PaquetesValidos, d.MinutosTrabajados),
                    //ExtraPeso = _kpiService.CalcularExtrapeso(d.PesoTotalReal, d.PesoObjetivo, d.PaquetesValidos),
                    HoraInicio = d.HoraInicioProduccion ?? DateTime.Now,
                    HoraFinAproximada = _kpiService.CalcularHoraFin(d.HoraInicioProduccion ?? DateTime.Now, d.PaquetesRequeridos ?? 0 - d.PaquetesValidos ?? 0, d.PaquetesValidos ?? 0 / d.MinutosTrabajados ?? 0),
                    PorcentajePedido = _kpiService.CalcularPorcentajePedido(d.PaquetesValidos ?? 0, d.PaquetesRequeridos ?? 0),
                    CosteMOD=_kpiService.CalcularCosteMOD(d.TotalHours ?? 0,d.CosteHora ?? 0, d.PaquetesValidos ?? 0, d.PesoObjetivo ?? 0),
                    ppm_objetivo=d.PpmObjetivo ?? 0
                })
                .ToList();

            return View(kpisLineas);
        }

    }
}
