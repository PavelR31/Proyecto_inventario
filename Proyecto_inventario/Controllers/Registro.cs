using Microsoft.AspNetCore.Mvc;
using Proyecto_inventario.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization; // Para la autorización de roles
using System.Security.Claims;

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

        // GET: Solicitar ser propietario
        [Authorize(Roles = "UsuarN")] // Solo "UsuarioN" puede acceder
        public IActionResult SolicitarPropietario()
        {
            return View();
        }

        // POST: Solicitar ser propietario
        
        // POST: Solicitar ser propietario
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "UsuarN")] // Solo "UsuarN" puede acceder
        
        public async Task<IActionResult> SolicitarPropietario(string confirmacion)
        {
            if (confirmacion == "true")
            {
                // Obtener el ID del usuario actual
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Verificar si userId se puede convertir a un entero
                if (int.TryParse(userId, out int usuarioId))
                {
                    // Obtener el usuario desde la base de datos
                    var usuario = await _context.Usuarios.FindAsync(usuarioId);

                    if (usuario == null)
                    {
                        return NotFound("Usuario no encontrado.");
                    }

                    // IDs de los roles
                    int rolPropietarioId = 5; // ID del rol "Propietario"
                    int usuarioNrolId = 4; // ID del rol "UsuarN"

                    // Eliminar el rol "UsuarN" del usuario si existe
                    var usuarioNrol = await _context.UsuarioRoles
                        .FirstOrDefaultAsync(ur => ur.UsuarioId == usuarioId && ur.RolId == usuarioNrolId);

                    if (usuarioNrol != null)
                    {
                        _context.UsuarioRoles.Remove(usuarioNrol);
                    }

                    // Verificar si el rol "Propietario" ya está asignado
                    var propietarioRol = await _context.UsuarioRoles
                        .FirstOrDefaultAsync(ur => ur.UsuarioId == usuarioId && ur.RolId == rolPropietarioId);

                    if (propietarioRol == null)
                    {
                        // Agregar el nuevo rol "Propietario" al usuario
                        var nuevoUsuarioRol = new UsuarioRol
                        {
                            UsuarioId = usuarioId,
                            RolId = rolPropietarioId
                        };

                        _context.UsuarioRoles.Add(nuevoUsuarioRol);
                    }

                    // Asegúrate de guardar los cambios en la base de datos
                    await _context.SaveChangesAsync();

                    // Insertar el usuario en la tabla Propietarios solo si no existe ya
                    var propietarioExistente = await _context.Propietarios
                        .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);

                    if (propietarioExistente == null)
                    {
                        var nuevoPropietario = new Propietario
                        {
                            UsuarioId = usuarioId
                        };

                        _context.Propietarios.Add(nuevoPropietario);
                        await _context.SaveChangesAsync();
                    }

                    // Guardar en TempData que el usuario se convirtió en propietario
                    TempData["PropietarioMessage"] = "¡Felicitaciones! Ahora eres Propietario.";

                    // Redirigir de nuevo a la vista de solicitud
                    return RedirectToAction("SolicitarPropietario");
                }
                else
                {
                    return NotFound("ID de usuario inválido.");
                }
            }

            return View();
        }



    }
}