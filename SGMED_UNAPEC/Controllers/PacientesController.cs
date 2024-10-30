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
    public class PacientesController : Controller
    {
        private readonly sgmed_unapecContext _context;

        public PacientesController(sgmed_unapecContext context)
        {
            _context = context;
        }

        // GET: Pacientes
        public async Task<IActionResult> Index()
        {
            var sgmed_unapecContext = _context.Pacientes.Include(p => p.Estado);
            return View(await sgmed_unapecContext.ToListAsync());
        }

        [HttpPost]
        public JsonResult ObtenerDatosPacientes()
        {
            try
            {
                int NroPeticion = Convert.ToInt32(Request.Form["draw"].FirstOrDefault() ?? "0");
                int CantidadRegistros = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
                int OmitirRegistros = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
                string ValorBuscado = Request.Form["search[value]"].FirstOrDefault() ?? "";

                IQueryable<Paciente> queryTbPaciente = _context.Pacientes.Include(p => p.Estado);

                if (!string.IsNullOrEmpty(ValorBuscado))
                {
                    queryTbPaciente = queryTbPaciente.Where(p =>
                        p.PacienteId.ToString().Contains(ValorBuscado) ||
                        p.Nombre.Contains(ValorBuscado) ||
                        p.Cedula.Contains(ValorBuscado) ||
                        p.NoCarnet.Contains(ValorBuscado) ||
                        p.TipoPaciente.Contains(ValorBuscado) ||
                        p.Estado.Descripcion.Contains(ValorBuscado));
                }

                int TotalRegistros = _context.Pacientes.Count();
                int TotalRegistrosFiltrados = queryTbPaciente.Count();

                var listaPacientes = queryTbPaciente
                    .Skip(OmitirRegistros)
                    .Take(CantidadRegistros)
                    .Select(p => new
                    {
                        p.PacienteId,
                        p.Nombre,
                        p.Cedula,
                        p.NoCarnet,
                        p.TipoPaciente,
                        Estado = p.Estado.Descripcion
                    })
                    .ToList();

                return Json(new
                {
                    draw = NroPeticion,
                    recordsTotal = TotalRegistros,
                    recordsFiltered = TotalRegistrosFiltrados,
                    data = listaPacientes
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

        // GET: Pacientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pacientes == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .Include(p => p.Estado)
                .FirstOrDefaultAsync(p => p.PacienteId == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // GET: Pacientes/Create
        public IActionResult Create()
        {
            ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "Descripcion");

            var estado = _context.Estados.ToList();
            ViewBag.Estado = estado;

            return View();
        }

        // POST: Pacientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Cedula,NoCarnet,TipoPaciente,EstadoId")] Paciente paciente)
        {
            var estado = _context.Estados.ToList();
            ViewBag.Estado = estado;
            //if (ModelState.IsValid)
            //{
            _context.Add(paciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            //ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "Descripcion", paciente.EstadoId);

            //var estado = _context.Estados.ToList();
            //ViewBag.Estado = estado;
            //return View(paciente);
        }

        // GET: Pacientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pacientes == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "Descripcion", paciente.EstadoId);
            return View(paciente);
        }

        // POST: Pacientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PacienteId,Nombre,Cedula,NoCarnet,TipoPaciente,EstadoId")] Paciente paciente)
        {
            if (id != paciente.PacienteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paciente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteExists(paciente.PacienteId))
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
            ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "Descripcion", paciente.EstadoId);
            return View(paciente);
        }

        // GET: Pacientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pacientes == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .Include(p => p.Estado)
                .FirstOrDefaultAsync(p => p.PacienteId == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // POST: Pacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pacientes == null)
            {
                return Problem("Entity set 'sgmed_unapecContext.Pacientes' is null.");
            }
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PacienteExists(int id)
        {
            return (_context.Pacientes?.Any(e => e.PacienteId == id)).GetValueOrDefault();
        }
    }
}
