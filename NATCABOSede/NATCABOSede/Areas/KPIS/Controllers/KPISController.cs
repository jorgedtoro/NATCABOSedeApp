using Microsoft.AspNetCore.Mvc;
using NATCABOSede.Services;

namespace NATCABOSede.Areas.KPIS.Controllers
{
    [Area("KPIS")]
    public class KPISController : Controller
    {
        private readonly KPIService _kpiService;

        public KPISController(KPIService kpiService)
        {
            _kpiService = kpiService;
        }
        public IActionResult Index()
        {

            // TODO: datos de prueba. Traer datos de DB.
            var paquetesTotales = 120;
            var minutosTrabajados = 60;
            var numeroPersonas = 5;
            var pesoReal = 0.10;
            var pesoObjetivo = 0.05;
            DateTime horaInicio = DateTime.Now.AddHours(-1); // Pedido comenzó hace 1 hora
            int paquetesProducidos = 60;
            double mediaPaquetesPorMinuto = 1.5;


            var ppm = _kpiService.CalcularPPM(paquetesTotales, minutosTrabajados, numeroPersonas);
            var pm = _kpiService.CalcularPM(paquetesTotales, minutosTrabajados);
            var extraPeso = _kpiService.CalcularExtrapeso(pesoReal, pesoObjetivo);

            var horaFinAproximada = _kpiService.CalcularHoraFin(horaInicio, paquetesTotales - paquetesProducidos, mediaPaquetesPorMinuto);
            var porcentajePedido = _kpiService.CalcularPorcentajePedido(paquetesProducidos, paquetesTotales);

            //Datos para paquetes
            ViewBag.PPM = ppm;
            ViewBag.PM = pm;
           
            //Datos para extrapeso
            ViewBag.ExtraPeso = extraPeso;
            
            //Datos para el pedido
            ViewBag.HoraInicio = horaInicio;
            ViewBag.HoraFinAproximada = horaFinAproximada;
            ViewBag.PorcentajePedido = porcentajePedido;

            return View();
        }
    }
}
