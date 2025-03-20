using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NATCABOSede.Models;
using NATCABOSede.Services;
using System.Threading.Tasks;

namespace NATCABOSede.Areas.KPIS.Controllers
{
    [Area("KPIS")]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string nombre, string password)
        {
            var user = await _authService.Authenticate(nombre, password);
            if (user == null)
            {
                ViewBag.Error = "Credenciales incorrectas";
                return View();
            }

            await _authService.SignIn(HttpContext, user);
            return RedirectToAction("Index", "KPIS", new { area = "KPIS" });
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.SignOut(HttpContext);
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
