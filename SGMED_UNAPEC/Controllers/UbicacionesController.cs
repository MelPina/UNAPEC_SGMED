using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGMED_UNAPEC.Models;

namespace SGMED_UNAPEC.Controllers
{
    public class UbicacionesController : Controller
    {
        private readonly sgmed_unapecContext _context;

        public UbicacionesController(sgmed_unapecContext context)
        {
            _context = context;
        }

        // GET: Ubicaciones
        public async Task<IActionResult> Index()
        {
            var sgmed_unapecContext = _context.Ubicacions.Include(u => u.Estado);
            return View(await sgmed_unapecContext.ToListAsync());
        }
        [HttpPost]
        public JsonResult ObtenerDatosUbicaciones()
        {
            try
            {
                // Representa el número de veces que se ha realizado una petición
                int NroPeticion = Convert.ToInt32(Request.Form["draw"].FirstOrDefault() ?? "0");

                // Cuantos registros va a devolver
                int CantidadRegistros = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");

                // Cuantos registros va a omitir
                int OmitirRegistros = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

                // El texto de búsqueda
                string ValorBuscado = Request.Form["search[value]"].FirstOrDefault() ?? "";

                // Consulta a la base de datos incluyendo la entidad Ubicacion y Estado
                IQueryable<Ubicacion> queryUbicacion = _context.Ubicacions
                    .Include(u => u.Estado)
                    .Include(u => u.Medicamentos);

                // Filtro de búsqueda
                if (!string.IsNullOrEmpty(ValorBuscado))
                {
                    queryUbicacion = queryUbicacion.Where(u =>
                        u.UbicacionId.ToString().Contains(ValorBuscado) ||
                        u.Descripcion.Contains(ValorBuscado) ||
                        u.Estante.Contains(ValorBuscado) ||
                        u.Tramo.Contains(ValorBuscado) ||
                        u.Celda.Contains(ValorBuscado) ||
                        u.Estado.Descripcion.Contains(ValorBuscado));
                }

                // Total de registros antes de filtrar
                int TotalRegistros = _context.Ubicacions.Count();

                // Total de registros filtrados
                int TotalRegistrosFiltrados = queryUbicacion.Count();

                // Obtener registros paginados
                var listaUbicaciones = queryUbicacion
                    .Skip(OmitirRegistros)
                    .Take(CantidadRegistros)
                    .Select(u => new
                    {
                        u.UbicacionId,
                        u.Descripcion,
                        u.Estante,
                        u.Tramo,
                        u.Celda,
                        Estado = u.Estado.Descripcion,
                        Medicamentos = u.Medicamentos.Select(m => new { m.MedicamentoId, m.Descripcion })
                    })
                    .ToList();

                // Devolver respuesta JSON esperada por DataTables
                return Json(new
                {
                    draw = NroPeticion,
                    recordsTotal = TotalRegistros,
                    recordsFiltered = TotalRegistrosFiltrados,
                    data = listaUbicaciones // Esta propiedad debe llamarse "data"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    error = "Ocurrió un error al obtener los datos: " + ex.Message
                });
            }
        }

        // GET: Ubicaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ubicacions == null)
            {
                return NotFound();
            }

            var ubicacion = await _context.Ubicacions
                .Include(u => u.Estado)
                .FirstOrDefaultAsync(m => m.UbicacionId == id);
            if (ubicacion == null)
            {
                return NotFound();
            }

            return View(ubicacion);
        }

        // GET: Ubicaciones/Create
        public IActionResult Create()
        {
            ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "EstadoId");
            var estado = _context.Estados.ToList();
            ViewBag.Estado = estado;
            return View();
        }

        // POST: Ubicaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UbicacionId,Descripcion,Estante,Tramo,Celda,EstadoId")] Ubicacion ubicacion)
        {
            
            {
                _context.Add(ubicacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "EstadoId", ubicacion.EstadoId);
            //var estado = _context.Estados.ToList();
            //ViewBag.Estado = estado;
            //return View(ubicacion);
        }

        // GET: Ubicaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ubicacions == null)
            {
                return NotFound();
            }

            var ubicacion = await _context.Ubicacions.FindAsync(id);
            if (ubicacion == null)
            {
                return NotFound();
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "EstadoId", ubicacion.EstadoId);
            var estado = _context.Estados.ToList();
            ViewBag.Estado = estado;
            return View(ubicacion);
        }

        // POST: Ubicaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UbicacionId,Descripcion,Estante,Tramo,Celda,EstadoId")] Ubicacion ubicacion)
        {
            if (id != ubicacion.UbicacionId)
            {
                return NotFound();
            }

            
            {
                try
                {
                    _context.Update(ubicacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UbicacionExists(ubicacion.UbicacionId))
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
            //var estado = _context.Estados.ToList();
            //ViewBag.Estado = estado;
            //ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "EstadoId", ubicacion.EstadoId);
            //return View(ubicacion);
        }

        // GET: Ubicaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ubicacions == null)
            {
                return NotFound();
            }

            var ubicacion = await _context.Ubicacions
                .Include(u => u.Estado)
                .FirstOrDefaultAsync(m => m.UbicacionId == id);
            if (ubicacion == null)
            {
                return NotFound();
            }

            return View(ubicacion);
        }

        // POST: Ubicaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ubicacions == null)
            {
                return Problem("Entity set 'sgmed_unapecContext.Ubicacions'  is null.");
            }
            var ubicacion = await _context.Ubicacions.FindAsync(id);
            if (ubicacion != null)
            {
                _context.Ubicacions.Remove(ubicacion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UbicacionExists(int id)
        {
          return (_context.Ubicacions?.Any(e => e.UbicacionId == id)).GetValueOrDefault();
        }
    }
}
