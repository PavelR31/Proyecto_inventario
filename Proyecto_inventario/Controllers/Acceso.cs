using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

using Proyecto_inventario.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Proyecto_inventario.Controllers
{
    public class Acceso : Controller
    {
        private readonly ApplicationDbContext _context;

        public Acceso(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult view()
        {
            return View();
        }
       
        public IActionResult registro()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(Usuario _usuario)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == _usuario.Correo && u.Clave == _usuario.Clave);

            if (usuario != null)
            {
                // Obtener roles del usuario
                var roles = await _context.UsuarioRoles
                    .Where(ur => ur.UsuarioId == usuario.Id)
                    .Join(_context.Roles, ur => ur.RolId, r => r.Id, (ur, r) => r.Nombre)
                    .ToListAsync();

                // Crear reclamaciones
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Nombre),
                    new Claim(ClaimTypes.Email, usuario.Correo),
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
                };

                // Añadir roles como reclamaciones
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                // Crear identidad de reclamaciones
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Iniciar sesión
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Si no se encuentra el usuario, vuelve a mostrar la vista de inicio
                ModelState.AddModelError("", "Correo o clave incorrectos.");
                return View();
            }
        }

        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
    }
}
