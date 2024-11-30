using Microsoft.AspNetCore.Mvc;
using NATCABOSede.Models;
using NATCABOSede.Services;
using NATCABOSede.ViewModels;

namespace NATCABOSede.Areas.KPIS.Controllers
{
    [Area("KPIS")]
    public class KPISController : Controller
    {
        private readonly NATCABOContext _context;
        private readonly KPIService _kpiService;

        public KPISController(NATCABOContext context, KPIService kpiService)
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
            //// TODO: datos de prueba. Traer datos de DB.
            //var paquetesTotales = 120;
            //var minutosTrabajados = 60;
            //var numeroPersonas = 5;
            //var pesoReal = 0.10;
            //var pesoObjetivo = 0.05;
            //DateTime horaInicio = DateTime.Now.AddHours(-1); // Pedido comenzó hace 1 hora
            //int paquetesProducidos = 60;
            //double mediaPaquetesPorMinuto = 1.5;

            //var tiempoTotal = 8.5; // Horas
            //var costeHora = 15.0; // €/h
            //var numeroPaquetes = 100;
            //var pesoMinimo = 0.5; // Kg por paquete


            //var ppm = _kpiService.CalcularPPM(paquetesTotales, minutosTrabajados, numeroPersonas);
            //var pm = _kpiService.CalcularPM(paquetesTotales, minutosTrabajados);
            //var extraPeso = _kpiService.CalcularExtrapeso(pesoReal, pesoObjetivo);

            //var horaFinAproximada = _kpiService.CalcularHoraFin(horaInicio, paquetesTotales - paquetesProducidos, mediaPaquetesPorMinuto);
            //var porcentajePedido = _kpiService.CalcularPorcentajePedido(paquetesProducidos, paquetesTotales);

            //var costeMOD = _kpiService.CalcularCosteMOD(tiempoTotal, costeHora, numeroPaquetes, pesoMinimo);
            ////Datos para paquetes
            //ViewBag.PPM = ppm;
            //ViewBag.PM = pm;

            ////Datos para extrapeso
            //ViewBag.ExtraPeso = extraPeso;

            ////Datos para el pedido
            //ViewBag.HoraInicio = horaInicio;
            //ViewBag.HoraFinAproximada = horaFinAproximada;
            //ViewBag.PorcentajePedido = porcentajePedido;

            ////Coste MOD
            //ViewBag.CosteMOD = costeMOD;
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
