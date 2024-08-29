using Microsoft.AspNetCore.Mvc;

using Proyecto_inventario.Models;
using Microsoft.EntityFrameworkCore;

namespace Proyecto_inventario.Controllers
{
    public class RegistroController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegistroController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Registro
        public IActionResult Index()
        {
            return View();
        }

        // POST: Registro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si el usuario ya existe
                    var existingUser = await _context.Usuarios
                        .AnyAsync(u => u.Correo == usuario.Correo);

                    if (existingUser)
                    {
                        ModelState.AddModelError("", "El correo electrónico ya está registrado.");
                        return View(usuario);
                    }

                    // Agregar el usuario a la base de datos
                    _context.Usuarios.Add(usuario);
                    await _context.SaveChangesAsync();

                    // Obtener el ID del rol UserN
                    var rolId = await _context.Roles
                        .Where(r => r.Nombre == "UsuarN")
                        .Select(r => r.Id)
                        .FirstOrDefaultAsync();

                    if (rolId > 0)
                    {
                        // Obtener el ID del usuario recién creado
                        var usuarioId = await _context.Usuarios
                            .Where(u => u.Correo == usuario.Correo)
                            .Select(u => u.Id)
                            .FirstOrDefaultAsync();

                        // Agregar la relación UsuarioRoles
                        var usuarioRol = new UsuarioRol
                        {
                            UsuarioId = usuarioId,
                            RolId = rolId
                        };

                        _context.UsuarioRoles.Add(usuarioRol);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Rol 'UsuarN' no encontrado.");
                        return View(usuario);
                    }

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    // Registra el error y muestra un mensaje adecuado
                    ModelState.AddModelError("", "Ocurrió un error al registrar el usuario.");
                    // Puedes registrar el error en un log para depuración
                    Console.WriteLine(ex.Message);
                }
            }

            // Si llegamos aquí, algo falló, volvemos a mostrar el formulario
            return View(usuario);
        }
    }
}
