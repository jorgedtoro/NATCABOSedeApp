using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NATCABOSede.Models;
using BCrypt.Net;

namespace NATCABOSede.Services
{
    public class AuthService
    {
        private readonly NATCABOContext _context;

        public AuthService(NATCABOContext context)
        {
            _context = context;
        }

        public async Task<Usuario> Authenticate(string nombre, string password)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == nombre);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }
            return user;
        }

        public async Task SignIn(HttpContext httpContext, Usuario usuario)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Rol)
        };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public async Task SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
