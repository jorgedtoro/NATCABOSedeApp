using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NATCABOSede.Interfaces;
using NATCABOSede.Models;
using NATCABOSede.Utilities;
using NATCABOSede.ViewModels;
using System.Linq;

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

        public IActionResult Index(short lineaSeleccionada)
        {
            var datos = _context.DatosKpisLives.FirstOrDefault(d => d.IdLinea == lineaSeleccionada);
            var modelo = GenerarViewModel(datos, lineaSeleccionada);
                       
            return View(modelo);
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


        private InfoDiscriminadoraViewModel GenerarViewModel(DatosKpisLive datos, short lineaSeleccionada)
        {
            if (datos == null)
            {
                return new InfoDiscriminadoraViewModel
                {
                    LineaSeleccionada = lineaSeleccionada
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

            return new InfoDiscriminadoraViewModel
            {
                LineaSeleccionada = lineaSeleccionada,
                NombreLinea = datos.NombreLinea ?? "Nombre de Linea",
                NombreCliente = datos.NombreCliente ?? "**CLIENTE**",
                NombreProducto = datos.NombreProducto ?? "**PRODUCTO**",
                PPM = datos.PpmMarco ?? 0,
                PPM_Disc = datos.PpmBizerba ?? 0,
                PMObjetivo = datos.PpmObjetivo ?? 0,
                ExtraPeso = _kpiService.CalcularExtrapeso(datos.PesoTotalReal ?? 0.0, datos.PesoObjetivo ?? 0.0, datos.PaquetesValidos ?? 0),
                HoraInicio = datos.HoraInicioProduccion ?? DateTime.Now,
                HoraFinAproximada = datos.HoraUltimoPaquete ?? DateTime.Now,
                PorcentajePedido = _kpiService.CalcularPorcentajePedido(datos.PaquetesValidos ?? 0, datos.PaquetesTotales ?? 0),
                CosteMOD = datos.CosteKg ?? 0,
                PersonalEnBalanza = datos.PersonalEnBalanza ?? 0,
                PersonalTotal = datos.PersonalTotal ?? 0,
                PersonalCorrecto = datos.PersonalCorrecto,
                PersonalPeriferico = datos.PersonalPeriferico ?? 0,
                Rangos_Ok = datos.RangosOk,
                DiscriminadorEnUso = datos.DiscriminadorEnUso,
                ExpulsionAire_Ok = datos.ExpulsionAireOk
            };
        }


    }
}
