using Microsoft.AspNetCore.Mvc;

namespace NATCABOSede.Areas.KPIS.Controllers
{
    [Area("KPIS")]
    public class ConfigurarObjetivosController : Controller
    {
        public IActionResult ObjetivosConfigurables()
        {
            return View();
        }
    }
}
