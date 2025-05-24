using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NATCABOSede.Interfaces;
using NATCABOSede.Models;
using NATCABOSede.Utilities;
using NATCABOSede.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NATCABOSede.Areas.KPIS.Controllers
{
    [Area("KPIS")]
    [Route("KPIS/[controller]/[action]")]
    public class InfoDiscriminadoraController : Controller
    {
        private readonly NATCABOContext _context;
        private readonly IKPIService _kpiService;
        private readonly ILogger<InfoDiscriminadoraController> _logger;

        public InfoDiscriminadoraController(NATCABOContext context, IKPIService kpiService, ILogger<InfoDiscriminadoraController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _kpiService = kpiService ?? throw new ArgumentNullException(nameof(kpiService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> Index(short lineaSeleccionada)
        {
            try
            {
                _logger.LogInformation($"Solicitando datos para la línea: {lineaSeleccionada}");
                
                var datos = await _context.DatosKpisLives
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.IdLinea == lineaSeleccionada);
                    
                if (datos == null)
                {
                    _logger.LogWarning($"No se encontraron datos para la línea: {lineaSeleccionada}");
                }
                
                var modelo = GenerarViewModel(datos, lineaSeleccionada);
                return View(modelo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener datos para la línea {lineaSeleccionada}");
                return StatusCode(500, "Ocurrió un error al procesar su solicitud. Por favor, intente nuevamente más tarde.");
            }
        }
        [HttpGet]
        public async Task<IActionResult> ObtenerLineasDisponibles()
        {
            try
            {
                _logger.LogInformation("Solicitando líneas disponibles");
                
                var lineas = await _context.DatosKpisLives
                    .Where(d => d.IdLinea.HasValue)
                    .Select(d => d.IdLinea.Value)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToListAsync();

                _logger.LogInformation($"Se encontraron {lineas.Count} líneas disponibles");
                return Json(lineas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener líneas disponibles");
                return StatusCode(500, new { error = "Error al obtener las líneas disponibles", details = ex.Message });
            }
        }


        private InfoDiscriminadoraViewModel GenerarViewModel(DatosKpisLive datos, short lineaSeleccionada)
        {
            try
            {
                if (datos == null)
                {
                    _logger.LogWarning($"No se encontraron datos para la línea {lineaSeleccionada}");
                    return new InfoDiscriminadoraViewModel
                    {
                        LineaSeleccionada = lineaSeleccionada,
                        NombreLinea = "Línea no encontrada",
                        NombreCliente = "No disponible",
                        NombreProducto = "No disponible"
                    };
                }

                double mediaPaquetesPorMinuto = 0.0;

                if (datos.HoraInicioProduccion.HasValue && datos.HoraUltimoPaquete.HasValue)
                {
                    var inicio = datos.HoraInicioProduccion.Value;
                    var fin = datos.HoraUltimoPaquete.Value;

                    TimeSpan diferencia = fin - inicio;
                    if (diferencia.TotalMinutes > 0)
                    {
                        mediaPaquetesPorMinuto = (datos.PaquetesValidos ?? 0) / diferencia.TotalMinutes;
                    }
                }

                var viewModel = new InfoDiscriminadoraViewModel
                {
                    LineaSeleccionada = lineaSeleccionada,
                    NombreLinea = datos.NombreLinea ?? "Sin nombre",
                    NombreCliente = datos.NombreCliente ?? "Sin cliente",
                    NombreProducto = datos.NombreProducto ?? "Sin producto",
                    PPM = datos.PpmLinea ?? 0,
                    PPM_Disc = datos.PpmBizerba ?? 0,
                    PMObjetivo = datos.PpmObj,
                    ExtraPeso = _kpiService.CalcularExtrapeso(datos.PesoTotalReal ?? 0.0, datos.PesoObjetivo ?? 0.0, datos.PaquetesValidos ?? 0),
                    HoraInicio = datos.HoraInicioProduccion ?? DateTime.Now,
                    HoraFinAproximada = _kpiService.CalcularHoraFin(
                        datos.HoraInicioProduccion ?? DateTime.Now,
                        (datos.PaquetesRequeridos ?? 0) - (datos.PaquetesValidos ?? 0),
                        datos.NPaquetes5min ?? 1.0
                    ),
                    PorcentajePedido = _kpiService.CalcularPorcentajePedido(datos.PaquetesValidos ?? 0, datos.PaquetesRequeridos ?? 0),
                    CosteMOD = datos.CosteKg ?? 0,
                    PersonalEnBalanza = datos.PersonalEnBalanza ?? 0,
                    PersonalTotal = datos.PersonalTotal ?? 0,
                    PersonalCorrecto = datos.PersonalCorrecto,
                    PersonalPeriferico = datos.PersonalPeriferico ?? 0,
                    Rangos_Ok = datos.RangosOk,
                    DiscriminadorEnUso = datos.DiscriminadorEnUso,
                    ExpulsionAire_Ok = datos.ExpulsionAireOk,
                    PpmCardClass = GetColorClass(datos.PpmLinea ?? 0, datos.PpmObj),
                    ArrowClass = GetArrow(datos.PpmLinea ?? 0, datos.NPaquetes5min ?? 0)
                };

                return viewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al generar el ViewModel para la línea {lineaSeleccionada}");
                return new InfoDiscriminadoraViewModel
                {
                    LineaSeleccionada = lineaSeleccionada,
                    NombreLinea = "Error",
                    NombreCliente = "Error al cargar datos",
                    NombreProducto = "Por favor, intente más tarde"
                };
            }
        }
        private string GetColorClass(double ppm, double ppmObjetivo)
        {
            

            if (ppm >= ppmObjetivo)
                return "bg-success";  // Verde
            else
                return "bg-danger";   // Rojo
        }
        private bool GetArrow(double ppm, double NPaquetes5min)
        {
            if (ppm >= NPaquetes5min)
                return true;
            else return false;
        }
    }
}
