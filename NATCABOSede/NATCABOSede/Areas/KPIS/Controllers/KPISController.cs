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

        public IActionResult Index(int lineaSeleccionada = 1)
        {
            ViewBag.LineaSeleccionada = lineaSeleccionada;

            var datos = ObtenerDatosPorLinea(lineaSeleccionada);

            if (datos == null)
            {
                return NotFound("No se encontraron datos para la línea seleccionada.");
            }
            var ppm = _kpiService.CalcularPPM(
                datos.PaquetesTrabajados ?? 0,
                datos.MinutosTrabajados ?? 0,
                datos.NumeroPersonas ?? 0);

            var pm = _kpiService.CalcularPM(
                datos.PaquetesTrabajados ?? 0,
                datos.MinutosTrabajados ?? 0);

            var extraPeso = _kpiService.CalcularExtrapeso(
                datos.PesoReal ?? 0,
                datos.PesoObjetivo ?? 0);

            var horaFinAproximada = _kpiService.CalcularHoraFin(
                datos.HoraInicio ?? DateTime.Now,
                (int)(datos.PaquetesTotales - datos.PaquetesProducidos ?? 0),
                datos.MediaPaquetesPorMinuto ?? 0);

            var porcentajePedido = _kpiService.CalcularPorcentajePedido(
                (int)(datos.PaquetesProducidos ?? 0),
                (int)(datos.PaquetesTotales ?? 0));

            var costeMOD = _kpiService.CalcularCosteMOD(
                datos.TiempoTotal ?? 0,
                datos.CosteHora ?? 0,
                datos.NumeroPaquetes ?? 0,
                datos.PesoMinimo ?? 0);
           
            var modelo = new DatosKpiViewModel
            {
                Cliente = "Nombre del Cliente",
                Producto = "Nombre del Producto",
                PPM = ppm,
                PM = pm,
                ExtraPeso = extraPeso,
                HoraInicio = datos.HoraInicio ?? DateTime.Now,
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
            var ppm = _kpiService.CalcularPPM(
                datos.PaquetesTrabajados ?? 0,
                datos.MinutosTrabajados ?? 0,
                datos.NumeroPersonas ?? 0);

            var pm = _kpiService.CalcularPM(
                datos.PaquetesTrabajados ?? 0,
                datos.MinutosTrabajados ?? 0);

            var extraPeso = _kpiService.CalcularExtrapeso(
                datos.PesoReal ?? 0,
                datos.PesoObjetivo ?? 0);

            var horaFinAproximada = _kpiService.CalcularHoraFin(
                datos.HoraInicio ?? DateTime.Now,
                (int)(datos.PaquetesTotales - datos.PaquetesProducidos ?? 0),
                datos.MediaPaquetesPorMinuto ?? 0);

            var porcentajePedido = _kpiService.CalcularPorcentajePedido(
                (int)(datos.PaquetesProducidos ?? 0),
                (int)(datos.PaquetesTotales ?? 0));

            var costeMOD = _kpiService.CalcularCosteMOD(
                datos.TiempoTotal ?? 0,
                datos.CosteHora ?? 0,
                datos.NumeroPaquetes ?? 0,
                datos.PesoMinimo ?? 0);

      
            var modelo = new DatosKpiViewModel
            {
                Cliente = "Nombre del Cliente", // TODO: obtener el nombre real usando el IdCliente
                Producto = "Nombre del Producto", // TODO: obtener el nombre real usando el IdProducto
                PPM = ppm,
                PM = pm,
                ExtraPeso = extraPeso,
                HoraInicio = datos.HoraInicio ?? DateTime.Now,
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
