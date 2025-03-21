using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NATCABOSede.Interfaces;
using NATCABOSede.Models;
using NATCABOSede.Utilities;
using NATCABOSede.ViewModels;

namespace NATCABOSede.Areas.KPIS.Controllers
{
    [Area("KPIS")]
    public class InfoDiscriminadoraController : Controller
    {
        private readonly NATCABOContext _context;
        private readonly IKPIService _kpiService;
        public InfoDiscriminadoraController(NATCABOContext context, IKPIService kpiService)
        {
            _context = context;
            _kpiService = kpiService;
        }
        public IActionResult Index(short lineaSeleccionada = 0)
        {
           
            var datos = ObtenerDatosPorLinea(lineaSeleccionada);
            DatosKpiViewModel modelo;
            if (datos == null) {
                 modelo = new DatosKpiViewModel
                {
                     Cliente = string.Empty,
                     Producto = string.Empty,
                     PPM = 0,
                     PPM_Disc = 0,
                     PM = 0,
                     PM_Disc = 0,
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
                modelo = _kpiService.GenerarDatosKpiViewModel(datos, mediaPaquetesPorMinuto);
            }
          

            return View(modelo);
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
        [HttpGet]
        public IActionResult ObtenerLineasDisponibles()
        {
            try
            {
                var lineas = _context.DatosKpisLives
                    .Select(d => d.IdLinea)
                    .Distinct()
                    .ToList();

                return Json(lineas);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error al obtener líneas disponibles.", ex);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

    }

}
