using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_inventario.Models;

namespace Proyecto_inventario.Controllers
{
    public class PropiedadsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PropiedadsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Propiedads
        public async Task<IActionResult> Index()
        {
            var userId = ObtenerIdUsuarioActual();
            if (userId == null || !int.TryParse(userId, out int userIdInt))
            {
                return Unauthorized(); // O redirige a una página de login
            }

            var propiedades = _context.Propiedades
                .Include(p => p.Propietario)
                .Where(p => p.Propietario.UsuarioId == userIdInt);

            return View(await propiedades.ToListAsync());
        }

        // GET: Propiedads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = ObtenerIdUsuarioActual();
            if (userId == null || !int.TryParse(userId, out int userIdInt))
            {
                return Unauthorized(); // O redirige a una página de login
            }

            var propiedad = await _context.Propiedades
                .Include(p => p.Propietario)
                .FirstOrDefaultAsync(p => p.Id == id && p.Propietario.UsuarioId == userIdInt);

            if (propiedad == null)
            {
                return NotFound();
            }

            return View(propiedad);
        }

        // GET: Propiedads/Create
        public IActionResult Create()
        {
            ViewData["PropietarioId"] = new SelectList(_context.Propietarios, "Id", "Id");
            return View();
        }

        // POST: Propiedads/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Direccion,PropietarioId")] Propiedad propiedad)
        {
            if (ModelState.IsValid)
            {
                var userId = ObtenerIdUsuarioActual();
                if (userId == null || !int.TryParse(userId, out int userIdInt))
                {
                    return Unauthorized(); // O redirige a una página de login
                }

                // Asigna el PropietarioId si no está asignado
                if (propiedad.PropietarioId == 0)
                {
                    propiedad.PropietarioId = userIdInt;
                }

                _context.Add(propiedad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PropietarioId"] = new SelectList(_context.Propietarios, "Id", "Id", propiedad.PropietarioId);
            return View(propiedad);
        }

        // GET: Propiedads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propiedad = await _context.Propiedades.FindAsync(id);
            if (propiedad == null)
            {
                return NotFound();
            }

            var userId = ObtenerIdUsuarioActual();
            if (userId == null || !int.TryParse(userId, out int userIdInt))
            {
                return Unauthorized(); // O redirige a una página de login
            }

            if (propiedad.PropietarioId != userIdInt)
            {
                return Unauthorized(); // O redirige a una página de error
            }

            ViewData["PropietarioId"] = new SelectList(_context.Propietarios, "Id", "Id", propiedad.PropietarioId);
            return View(propiedad);
        }

        // POST: Propiedads/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Direccion,PropietarioId")] Propiedad propiedad)
        {
            if (id != propiedad.Id)
            {
                return NotFound();
            }

            var userId = ObtenerIdUsuarioActual();
            if (userId == null || !int.TryParse(userId, out int userIdInt))
            {
                return Unauthorized(); // O redirige a una página de login
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (propiedad.PropietarioId != userIdInt)
                    {
                        return Unauthorized(); // O redirige a una página de error
                    }

                    _context.Update(propiedad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropiedadExists(propiedad.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PropietarioId"] = new SelectList(_context.Propietarios, "Id", "Id", propiedad.PropietarioId);
            return View(propiedad);
        }

        // GET: Propiedads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = ObtenerIdUsuarioActual();
            if (userId == null || !int.TryParse(userId, out int userIdInt))
            {
                return Unauthorized(); // O redirige a una página de login
            }

            var propiedad = await _context.Propiedades
                .Include(p => p.Propietario)
                .FirstOrDefaultAsync(p => p.Id == id && p.Propietario.UsuarioId == userIdInt);

            if (propiedad == null)
            {
                return NotFound();
            }

            return View(propiedad);
        }

        // POST: Propiedads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = ObtenerIdUsuarioActual();
            if (userId == null || !int.TryParse(userId, out int userIdInt))
            {
                return Unauthorized(); // O redirige a una página de login
            }

            var propiedad = await _context.Propiedades
                .Where(p => p.Id == id && p.Propietario.UsuarioId == userIdInt)
                .FirstOrDefaultAsync();

            if (propiedad != null)
            {
                _context.Propiedades.Remove(propiedad);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PropiedadExists(int id)
        {
            return _context.Propiedades.Any(e => e.Id == id);
        }

        private string ObtenerIdUsuarioActual()
        {
            // Implementa la lógica para obtener el ID del usuario actual
            // Esto puede ser desde la sesión, cookie o JWT, dependiendo de cómo gestiones la autenticación
            return HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
