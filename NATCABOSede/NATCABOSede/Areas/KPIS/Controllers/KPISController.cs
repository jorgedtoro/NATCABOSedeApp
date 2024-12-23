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

        public IActionResult Index(int lineaSeleccionada = 3)
        {
            ViewBag.LineaSeleccionada = lineaSeleccionada;

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

                // Resta de DateTimes => TimeSpan
                TimeSpan diferencia = fin - inicio;

               
                mediaPaquetesPorMinuto = diferencia.TotalMinutes;  // Convertir a minutos para el tipo double del método.
            }



            var ppm = _kpiService.CalcularPPM(
                datos.PaquetesValidos ?? 0,
                datos.MinutosTrabajados ?? 0,
                datos.NumeroOperadores ?? 0);

            var pm = _kpiService.CalcularPM(
                datos.PaquetesValidos ?? 0,
                datos.MinutosTrabajados ?? 0);

            var extraPeso = _kpiService.CalcularExtrapeso(
                datos.PesoTotalReal ?? 0,
                datos.PesoObjetivo ?? 0);

            var horaFinAproximada = _kpiService.CalcularHoraFin(
                datos.HoraInicioProduccion ?? DateTime.Now,
                (int)(datos.PaquetesTotales - datos.PaquetesValidos ?? 0),
                mediaPaquetesPorMinuto);

            var porcentajePedido = _kpiService.CalcularPorcentajePedido(
                (int)(datos.PaquetesValidos ?? 0),
                (int)(datos.PaquetesTotales ?? 0));

           
            var costeMOD = _kpiService.CalcularCosteMOD(
                datos.MinutosTrabajados ?? 0, //antes TiempoTotal
                datos.CosteHora ?? 0,
                datos.PaquetesRequeridos ?? 0, //antes NumeroPaquetes
                datos.PesoObjetivo ?? 0); //antes pesoMinimo

            var modelo = new DatosKpiViewModel
            {
                Cliente = "Nombre del Cliente",
                Producto = "Nombre del Producto",
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
        public IActionResult ObtenerKPIs(int lineaSeleccionada)
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

                // Resta de DateTimes => TimeSpan
                TimeSpan diferencia = fin - inicio;


                mediaPaquetesPorMinuto = diferencia.TotalMinutes;  // Convertir a minutos para el tipo double del método.
            }

            var ppm = _kpiService.CalcularPPM(
                datos.PaquetesValidos ?? 0,
                datos.MinutosTrabajados ?? 0,
                datos.NumeroOperadores ?? 0);

            var pm = _kpiService.CalcularPM(
                datos.PaquetesValidos ?? 0,
                datos.MinutosTrabajados ?? 0);

            var extraPeso = _kpiService.CalcularExtrapeso(
                datos.PesoTotalReal ?? 0,
                datos.PesoObjetivo ?? 0);

            var horaFinAproximada = _kpiService.CalcularHoraFin(
                datos.HoraInicioProduccion ?? DateTime.Now,
                (int)(datos.PaquetesTotales - datos.PaquetesValidos ?? 0),
                 mediaPaquetesPorMinuto);

            var porcentajePedido = _kpiService.CalcularPorcentajePedido(
                (int)(datos.PaquetesValidos ?? 0),
                (int)(datos.PaquetesTotales ?? 0));

            var costeMOD = _kpiService.CalcularCosteMOD(
                datos.MinutosTrabajados ?? 0, //antes TiempoTotal
                datos.CosteHora ?? 0,
                datos.PaquetesRequeridos ?? 0, //antes NumeroPaquetes
                datos.PesoObjetivo ?? 0); //antes pesoMinimo

      
            var modelo = new DatosKpiViewModel
            {
                Cliente = "Nombre del Cliente", // TODO: obtener el nombre real usando el IdCliente
                Producto = "Nombre del Producto", // TODO: obtener el nombre real usando el IdProducto
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
        private DatosKpi ObtenerDatosPorLinea(int linea)
        {
            var datosKPI = _context.DatosKpis
            .FirstOrDefault(d => d.IdLinea == linea);

            return datosKPI;
        }
    }


}
