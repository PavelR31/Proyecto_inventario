using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using Proyecto_inventario.Models;

using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace Proyecto_inventario.Controllers
{
    //2.- AÑADIR LA AUTHORIZACION
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize(Roles = "Administrador,Supervisor,Empleado,Jefe, UsuarN")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Productos()
        {
            return View();
        }

        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Ventas()
        {
            return View();
        }

        

        [Authorize(Roles = "Administrador,Supervisor")]
        public IActionResult Compras()
        {
            return View();
        }

        [Authorize(Roles = "Administrador,Supervisor")]
        public IActionResult Clientes()
        {
            return View();
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