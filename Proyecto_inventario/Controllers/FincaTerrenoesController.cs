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
    public class FincaTerrenoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FincaTerrenoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FincaTerrenoes
        public async Task<IActionResult> Index()
        {
            var userId = ObtenerIdUsuarioActual();
            if (userId == null || !int.TryParse(userId, out int userIdInt))
            {
                return Unauthorized(); // O redirige a una página de login
            }

            var fincasTerrenos = _context.FincasTerrenos
                .Include(f => f.Propietario)
                .Where(f => f.Propietario.UsuarioId == userIdInt);

            return View(await fincasTerrenos.ToListAsync());
        }

        // GET: FincaTerrenoes/Details/5
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

            var fincaTerreno = await _context.FincasTerrenos
                .Include(f => f.Propietario)
                .FirstOrDefaultAsync(f => f.Id == id && f.Propietario.UsuarioId == userIdInt);

            if (fincaTerreno == null)
            {
                return NotFound();
            }

            return View(fincaTerreno);
        }

        // GET: FincaTerrenoes/Create
        public IActionResult Create()
        {
            ViewData["PropietarioId"] = new SelectList(_context.Propietarios, "Id", "Id");
            return View();
        }

        // POST: FincaTerrenoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Direccion,Area,Tipo,PropietarioId")] FincaTerreno fincaTerreno)
        {
            if (ModelState.IsValid)
            {
                var userId = ObtenerIdUsuarioActual();
                if (userId == null || !int.TryParse(userId, out int userIdInt))
                {
                    return Unauthorized(); // O redirige a una página de login
                }

                // Asigna el PropietarioId si no está asignado
                if (fincaTerreno.PropietarioId == 0)
                {
                    fincaTerreno.PropietarioId = userIdInt;
                }

                _context.Add(fincaTerreno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PropietarioId"] = new SelectList(_context.Propietarios, "Id", "Id", fincaTerreno.PropietarioId);
            return View(fincaTerreno);
        }

        // GET: FincaTerrenoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fincaTerreno = await _context.FincasTerrenos.FindAsync(id);
            if (fincaTerreno == null)
            {
                return NotFound();
            }

            var userId = ObtenerIdUsuarioActual();
            if (userId == null || !int.TryParse(userId, out int userIdInt))
            {
                return Unauthorized(); // O redirige a una página de login
            }

            if (fincaTerreno.PropietarioId != userIdInt)
            {
                return Unauthorized(); // O redirige a una página de error
            }

            ViewData["PropietarioId"] = new SelectList(_context.Propietarios, "Id", "Id", fincaTerreno.PropietarioId);
            return View(fincaTerreno);
        }

        // POST: FincaTerrenoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Direccion,Area,Tipo,PropietarioId")] FincaTerreno fincaTerreno)
        {
            if (id != fincaTerreno.Id)
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
                    if (fincaTerreno.PropietarioId != userIdInt)
                    {
                        return Unauthorized(); // O redirige a una página de error
                    }

                    _context.Update(fincaTerreno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FincaTerrenoExists(fincaTerreno.Id))
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
            ViewData["PropietarioId"] = new SelectList(_context.Propietarios, "Id", "Id", fincaTerreno.PropietarioId);
            return View(fincaTerreno);
        }

        // GET: FincaTerrenoes/Delete/5
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

            var fincaTerreno = await _context.FincasTerrenos
                .Include(f => f.Propietario)
                .FirstOrDefaultAsync(f => f.Id == id && f.Propietario.UsuarioId == userIdInt);

            if (fincaTerreno == null)
            {
                return NotFound();
            }

            return View(fincaTerreno);
        }

        // POST: FincaTerrenoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = ObtenerIdUsuarioActual();
            if (userId == null || !int.TryParse(userId, out int userIdInt))
            {
                return Unauthorized(); // O redirige a una página de login
            }

            var fincaTerreno = await _context.FincasTerrenos
                .Where(f => f.Id == id && f.Propietario.UsuarioId == userIdInt)
                .FirstOrDefaultAsync();

            if (fincaTerreno != null)
            {
                _context.FincasTerrenos.Remove(fincaTerreno);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool FincaTerrenoExists(int id)
        {
            return _context.FincasTerrenos.Any(e => e.Id == id);
        }

        private string ObtenerIdUsuarioActual()
        {
            // Implementa la lógica para obtener el ID del usuario actual
            return HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
