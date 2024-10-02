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
    public class TipofarmacosController : Controller
    {
        private readonly sgmed_unapecContext _context;

        public TipofarmacosController(sgmed_unapecContext context)
        {
            _context = context;
        }

        // GET: Tipofarmacos
        public async Task<IActionResult> Index()
        {
            var sgmed_unapecContext = _context.Tipofarmacos.Include(t => t.Estado);
            return View(await sgmed_unapecContext.ToListAsync());
        }
        public JsonResult ObtenerDatosTiposFarmacos()
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

                // Consulta de tipos de fármacos incluyendo el estado
                IQueryable<Tipofarmaco> queryTbTipofarmaco = _context.Tipofarmacos.Include(tf => tf.Estado);

                // Filtro de búsqueda
                if (!string.IsNullOrEmpty(ValorBuscado))
                {
                    queryTbTipofarmaco = queryTbTipofarmaco.Where(tf =>
                        tf.TipoFarmacoId.ToString().Contains(ValorBuscado) ||
                        tf.Descripcion.Contains(ValorBuscado) ||
                        tf.Estado.Descripcion.Contains(ValorBuscado));
                }

                // Total de registros antes de filtrar
                int TotalRegistros = _context.Tipofarmacos.Count();

                // Total de registros filtrados
                int TotalRegistrosFiltrados = queryTbTipofarmaco.Count();

                // Obtener registros paginados
                var listaTiposFarmacos = queryTbTipofarmaco
                    .Skip(OmitirRegistros)
                    .Take(CantidadRegistros)
                    .Select(tf => new
                    {
                        tf.TipoFarmacoId,
                        tf.Descripcion,
                        Estado = tf.Estado.Descripcion
                    })
                    .ToList();

                // Devolver respuesta JSON esperada por DataTables
                return Json(new
                {
                    draw = NroPeticion,
                    recordsTotal = TotalRegistros,
                    recordsFiltered = TotalRegistrosFiltrados,
                    data = listaTiposFarmacos // Esta propiedad debe llamarse "data"
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
        // GET: Tipofarmacos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tipofarmacos == null)
            {
                return NotFound();
            }

            var tipofarmaco = await _context.Tipofarmacos
                .Include(t => t.Estado)
                .FirstOrDefaultAsync(m => m.TipoFarmacoId == id);
            if (tipofarmaco == null)
            {
                return NotFound();
            }

            return View(tipofarmaco);
        }

        // GET: Tipofarmacos/Create
        public IActionResult Create()
        {
            ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "EstadoId");
            var estado = _context.Estados.ToList();
            ViewBag.Estado = estado;
            return View();
        }

        // POST: Tipofarmacos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TipoFarmacoId,Descripcion,EstadoId")] Tipofarmaco tipofarmaco)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipofarmaco);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "EstadoId", tipofarmaco.EstadoId);
            return View(tipofarmaco);
        }

        // GET: Tipofarmacos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tipofarmacos == null)
            {
                return NotFound();
            }

            var tipofarmaco = await _context.Tipofarmacos.FindAsync(id);
            if (tipofarmaco == null)
            {
                return NotFound();
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "EstadoId", tipofarmaco.EstadoId);
            var estado = _context.Estados.ToList();
            ViewBag.Estado = estado;
            return View(tipofarmaco);
        }

        // POST: Tipofarmacos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TipoFarmacoId,Descripcion,EstadoId")] Tipofarmaco tipofarmaco)
        {
            if (id != tipofarmaco.TipoFarmacoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipofarmaco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipofarmacoExists(tipofarmaco.TipoFarmacoId))
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
            ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "EstadoId", tipofarmaco.EstadoId);
            var estado = _context.Estados.ToList();
            ViewBag.Estado = estado;
            return View(tipofarmaco);
        }

        // GET: Tipofarmacos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tipofarmacos == null)
            {
                return NotFound();
            }

            var tipofarmaco = await _context.Tipofarmacos
                .Include(t => t.Estado)
                .FirstOrDefaultAsync(m => m.TipoFarmacoId == id);
            if (tipofarmaco == null)
            {
                return NotFound();
            }

            return View(tipofarmaco);
        }

        // POST: Tipofarmacos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tipofarmacos == null)
            {
                return Problem("Entity set 'sgmed_unapecContext.Tipofarmacos'  is null.");
            }
            var tipofarmaco = await _context.Tipofarmacos.FindAsync(id);
            if (tipofarmaco != null)
            {
                _context.Tipofarmacos.Remove(tipofarmaco);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipofarmacoExists(int id)
        {
          return (_context.Tipofarmacos?.Any(e => e.TipoFarmacoId == id)).GetValueOrDefault();
        }
    }
}
