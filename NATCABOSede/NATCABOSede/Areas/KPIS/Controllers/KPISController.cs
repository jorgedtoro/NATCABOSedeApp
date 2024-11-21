using Microsoft.AspNetCore.Mvc;

namespace NATCABOSede.Areas.KPIS.Controllers
{
    [Area("KPIS")]
    public class KPISController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
