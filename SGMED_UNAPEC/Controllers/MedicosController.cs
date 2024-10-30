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
    public class MedicosController : Controller
    {
        private readonly sgmed_unapecContext _context;

        public MedicosController(sgmed_unapecContext context)
        {
            _context = context;
        }

        // GET: Medicos
        public async Task<IActionResult> Index()
        {
            var sgmed_unapecContext = _context.Medicos.Include(m => m.Estado);
            return View(await sgmed_unapecContext.ToListAsync());
        }

        [HttpPost]
        public JsonResult ObtenerDatosMedicos()
        {
            try
            {
                int NroPeticion = Convert.ToInt32(Request.Form["draw"].FirstOrDefault() ?? "0");
                int CantidadRegistros = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
                int OmitirRegistros = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
                string ValorBuscado = Request.Form["search[value]"].FirstOrDefault() ?? "";

                IQueryable<Medico> queryTbMedico = _context.Medicos.Include(m => m.Estado);

                if (!string.IsNullOrEmpty(ValorBuscado))
                {
                    queryTbMedico = queryTbMedico.Where(m =>
                        m.MedicoId.ToString().Contains(ValorBuscado) ||
                        m.Nombre.Contains(ValorBuscado) ||
                        m.Cedula.Contains(ValorBuscado) ||
                        m.TandaLabor.Contains(ValorBuscado) ||
                        m.Especialidad.Contains(ValorBuscado) ||
                        m.Estado.Descripcion.Contains(ValorBuscado));
                }

                int TotalRegistros = _context.Medicos.Count();
                int TotalRegistrosFiltrados = queryTbMedico.Count();

                var listaMedicos = queryTbMedico
                    .Skip(OmitirRegistros)
                    .Take(CantidadRegistros)
                    .Select(m => new
                    {
                        m.MedicoId,
                        m.Nombre,
                        m.Cedula,
                        m.TandaLabor,
                        m.Especialidad,
                        Estado = m.Estado.Descripcion
                    })
                    .ToList();

                return Json(new
                {
                    draw = NroPeticion,
                    recordsTotal = TotalRegistros,
                    recordsFiltered = TotalRegistrosFiltrados,
                    data = listaMedicos
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

        // GET: Medicos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Medicos == null)
            {
                return NotFound();
            }

            var medico = await _context.Medicos
                .Include(m => m.Estado)
                .FirstOrDefaultAsync(m => m.MedicoId == id);
            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }

        // GET: Medicos/Create
        public IActionResult Create()
        {
            ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "Descripcion");

            var estado = _context.Estados.ToList();
            ViewBag.Estado = estado;
            return View();
        }

        // POST: Medicos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Cedula,TandaLabor,Especialidad,EstadoId")] Medico medico)
        {
            var estado = _context.Estados.ToList();
            ViewBag.Estado = estado;
            if (ModelState.IsValid)
            {
                _context.Add(medico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "Descripcion", medico.EstadoId);
            return View(medico);
        }

        // GET: Medicos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Medicos == null)
            {
                return NotFound();
            }

            var medico = await _context.Medicos.FindAsync(id);
            if (medico == null)
            {
                return NotFound();
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "Descripcion", medico.EstadoId);
            return View(medico);
        }

        // POST: Medicos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MedicoId,Nombre,Cedula,TandaLabor,Especialidad,EstadoId")] Medico medico)
        {
            if (id != medico.MedicoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicoExists(medico.MedicoId))
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
            ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "Descripcion", medico.EstadoId);
            return View(medico);
        }

        // GET: Medicos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Medicos == null)
            {
                return NotFound();
            }

            var medico = await _context.Medicos
                .Include(m => m.Estado)
                .FirstOrDefaultAsync(m => m.MedicoId == id);
            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }

        // POST: Medicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Medicos == null)
            {
                return Problem("Entity set 'sgmed_unapecContext.Medicos' is null.");
            }
            var medico = await _context.Medicos.FindAsync(id);
            if (medico != null)
            {
                _context.Medicos.Remove(medico);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicoExists(int id)
        {
            return (_context.Medicos?.Any(e => e.MedicoId == id)).GetValueOrDefault();
        }
    }
}
