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
    public class MarcasController : Controller
    {
        private readonly sgmed_unapecContext _context;

        public MarcasController(sgmed_unapecContext context)
        {
            _context = context;
        }

        // GET: Marcas
        public async Task<IActionResult> Index()
        {
            var sgmed_unapecContext = _context.Marcas.Include(m => m.Estado);
            return View(await sgmed_unapecContext.ToListAsync());
        }

        [HttpPost]
        public JsonResult ObtenerDatosMarcas()
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

                IQueryable<Marca> queryTbMarca = _context.Marcas.Include(m => m.Estado);

                // Filtro de búsqueda
                if (!string.IsNullOrEmpty(ValorBuscado))
                {
                    queryTbMarca = queryTbMarca.Where(m =>
                        m.MarcaId.ToString().Contains(ValorBuscado) ||
                        m.Descripcion.Contains(ValorBuscado) ||
                        m.Estado.Descripcion.Contains(ValorBuscado));
                }

                // Total de registros antes de filtrar
                int TotalRegistros = _context.Marcas.Count();

                // Total de registros filtrados
                int TotalRegistrosFiltrados = queryTbMarca.Count();

                // Obtener registros paginados
                var listaMarcas = queryTbMarca
                    .Skip(OmitirRegistros)
                    .Take(CantidadRegistros)
                    .Select(m => new
                    {
                        m.MarcaId,
                        m.Descripcion,
                        Estado = m.Estado.Descripcion
                    })
                    .ToList();

                // Devolver respuesta JSON esperada por DataTables
                return Json(new
                {
                    draw = NroPeticion,
                    recordsTotal = TotalRegistros,
                    recordsFiltered = TotalRegistrosFiltrados,
                    data = listaMarcas // Esta propiedad debe llamarse "data"
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


        // GET: Marcas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Marcas == null)
            {
                return NotFound();
            }

            var marca = await _context.Marcas
                .Include(m => m.Estado)
                .FirstOrDefaultAsync(m => m.MarcaId == id);
            if (marca == null)
            {
                return NotFound();
            }

            return View(marca);
        }

        // GET: Marcas/Create
        public IActionResult Create()
        {
            ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "EstadoId");
            var estado = _context.Estados.ToList();
            ViewBag.Estado = estado;
            return View();
        }

        // POST: Marcas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MarcaId,Descripcion,EstadoId")] Marca marca)
        {
            if (ModelState.IsValid)
            {
                _context.Add(marca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "EstadoId", marca.EstadoId);
            var estado = _context.Estados.ToList();
            ViewBag.Estado = estado;
            return View(marca);
        }

        // GET: Marcas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Marcas == null)
            {
                return NotFound();
            }

            var marca = await _context.Marcas.FindAsync(id);
            if (marca == null)
            {
                return NotFound();
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "EstadoId", marca.EstadoId);
            var estado = _context.Estados.ToList();
            ViewBag.Estado = estado;
            return View(marca);
        }

        // POST: Marcas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MarcaId,Descripcion,EstadoId")] Marca marca)
        {
            if (id != marca.MarcaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(marca);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarcaExists(marca.MarcaId))
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
            var estado = _context.Estados.ToList();
            ViewBag.Estado = estado;
            ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "EstadoId", marca.EstadoId);
            return View(marca);
        }

        // GET: Marcas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Marcas == null)
            {
                return NotFound();
            }

            var marca = await _context.Marcas
                .Include(m => m.Estado)
                .FirstOrDefaultAsync(m => m.MarcaId == id);
            if (marca == null)
            {
                return NotFound();
            }

            return View(marca);
        }

        // POST: Marcas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Marcas == null)
            {
                return Problem("Entity set 'sgmed_unapecContext.Marcas'  is null.");
            }
            var marca = await _context.Marcas.FindAsync(id);
            if (marca != null)
            {
                _context.Marcas.Remove(marca);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarcaExists(int id)
        {
          return (_context.Marcas?.Any(e => e.MarcaId == id)).GetValueOrDefault();
        }
    }
}
