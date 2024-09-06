using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Proyecto_inventario.Models;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Threading.Tasks;

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Proyecto_inventario.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private const int PageSize = 10; // Número de propiedades por página

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Authorize(Roles = "Administrador,Supervisor,Empleado,Jefe, UsuarN")]
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 10; // Ajusta el tamaño de la página según sea necesario
            var propiedades = await _context.Propiedades
                .Include(p => p.Propietario)
                .ThenInclude(pr => pr.Usuario)
                .ToListAsync();

            var model = PaginatedList<Propiedad>.Create(propiedades, page, pageSize);
            return View(model);
        }


        public IActionResult Propetario()
        {
            return View();
        }



        // Otros métodos...

        public async Task<IActionResult> Logout()
        {
            // Finaliza la sesión del usuario
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirige al usuario a la página de inicio
            return RedirectToAction("Index");
        }


        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
