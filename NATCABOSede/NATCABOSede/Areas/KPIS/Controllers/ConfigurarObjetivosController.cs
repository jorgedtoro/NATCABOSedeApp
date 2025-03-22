using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NATCABOSede.Areas.KPIS.Controllers
{
    [Area("KPIS")]
    public class ConfigurarObjetivosController : Controller
    {
      // [Authorize]
        public IActionResult ObjetivosConfigurables()
        {
            return View();
        }
    }
}
