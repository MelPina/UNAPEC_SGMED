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
    public class RegistrovisitasController : Controller
    {
        private readonly sgmed_unapecContext _context;

        public RegistrovisitasController(sgmed_unapecContext context)
        {
            _context = context;
        }

        // GET: Registrovisitas
        public async Task<IActionResult> Index()
        {
            var sgmed_unapecContext = _context.Registrovisita.Include(r => r.Estado).Include(r => r.Medicamento).Include(r => r.Medico).Include(r => r.Paciente);
            return View(await sgmed_unapecContext.ToListAsync());
        }
        [HttpPost]
        public JsonResult ObtenerDatosVisitas()
        {
            try
            {
                // Representa el número de veces que se ha realizado una petición
                int NroPeticion = Convert.ToInt32(Request.Form["draw"].FirstOrDefault() ?? "0");

                // Cuantos registros va a devolver
                int CantidadRegistros = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");

                // Cuantos registros va a omitir
                int OmitirRegistros = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

                //// El texto de búsqueda
                string ValorBuscado = Request.Form["search[value]"].FirstOrDefault() ?? "";

            
                IQueryable<Registrovisitum> queryVisitas = _context.Registrovisita
                .Include(v => v.Estado)
                .Include(v => v.Medico)
                .Include(v => v.Paciente)
                .Include(v => v.Medicamento);

                if (!string.IsNullOrEmpty(ValorBuscado))
                {
                    queryVisitas = queryVisitas.Where(v =>
                        (v.Sintomas != null && v.Sintomas.Contains(ValorBuscado)) || 
                        (v.Medico != null && EF.Functions.Like(v.Medico.Nombre, $"%{ValorBuscado}%")) ||
                        (v.Paciente != null && EF.Functions.Like(v.Paciente.Nombre, $"%{ValorBuscado}%")));
                }

              


                // Total de registros antes de filtrar
                int TotalRegistros = _context.Registrovisita.Count();

                // Total de registros filtrados
                int TotalRegistrosFiltrados = queryVisitas.Count();

                // Obtener registros paginados
                var listaVisitas = queryVisitas
                    .Skip(OmitirRegistros)
                    .Take(CantidadRegistros)
                    .Select(v => new
                    {
                        v.VisitaId,
                        Medico = v.Medico.Nombre,
                        Paciente = v.Paciente.Nombre,
                        FechaVisita = v.FechaVisita.ToString("yyyy-MM-dd"),
                        HoraVisita = v.HoraVisita.ToString("HH:mm"),
                        v.Sintomas,
                        Medicamento = v.Medicamento.Descripcion,
                        v.Recomendaciones,
                        Estado = v.Estado.Descripcion
                    })
                    .ToList();

                // Devolver respuesta JSON esperada por DataTables
                return Json(new
                {
                    draw = NroPeticion,
                    recordsTotal = TotalRegistros,
                    recordsFiltered = TotalRegistrosFiltrados,
                    data = listaVisitas // Esta propiedad debe llamarse "data"
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




        // GET: Registrovisitas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Registrovisita == null)
            {
                return NotFound();
            }

            var registrovisitum = await _context.Registrovisita
                .Include(r => r.Estado)
                .Include(r => r.Medicamento)
                .Include(r => r.Medico)
                .Include(r => r.Paciente)
                .FirstOrDefaultAsync(m => m.VisitaId == id);
            if (registrovisitum == null)
            {
                return NotFound();
            }

            return View(registrovisitum);
        }

        // GET: Registrovisitas/Create
        public IActionResult Create()
        {
            //ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "EstadoId");
            var estado = _context.Estados.ToList();
            ViewBag.Estado = estado;
            ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "MedicamentoId", "MedicamentoId");
            ViewData["MedicoId"] = new SelectList(_context.Medicos, "MedicoId", "MedicoId");
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "PacienteId");
            return View();
        }

        // POST: Registrovisitas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VisitaId,MedicoId,PacienteId,FechaVisita,HoraVisita,Sintomas,MedicamentoId,Recomendaciones,EstadoId")] Registrovisitum registrovisitum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registrovisitum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "EstadoId", registrovisitum.EstadoId);
            var estado = _context.Estados.ToList();
            ViewBag.Estado = estado;
            ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "MedicamentoId", "MedicamentoId", registrovisitum.MedicamentoId);
            ViewData["MedicoId"] = new SelectList(_context.Medicos, "MedicoId", "MedicoId", registrovisitum.MedicoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "PacienteId", registrovisitum.PacienteId);
            return View(registrovisitum);
        }

        // GET: Registrovisitas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Registrovisita == null)
            {
                return NotFound();
            }

            var registrovisitum = await _context.Registrovisita.FindAsync(id);
            if (registrovisitum == null)
            {
                return NotFound();
            }
            //ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "EstadoId", registrovisitum.EstadoId);
            var estado = _context.Estados.ToList();
            ViewBag.Estado = estado;
            ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "MedicamentoId", "MedicamentoId", registrovisitum.MedicamentoId);
            ViewData["MedicoId"] = new SelectList(_context.Medicos, "MedicoId", "MedicoId", registrovisitum.MedicoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "PacienteId", registrovisitum.PacienteId);
            return View(registrovisitum);
        }

        // POST: Registrovisitas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VisitaId,MedicoId,PacienteId,FechaVisita,HoraVisita,Sintomas,MedicamentoId,Recomendaciones,EstadoId")] Registrovisitum registrovisitum)
        {
            if (id != registrovisitum.VisitaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registrovisitum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrovisitumExists(registrovisitum.VisitaId))
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
            //ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "EstadoId", registrovisitum.EstadoId);
            var estado = _context.Estados.ToList();
            ViewBag.Estado = estado;
            ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "MedicamentoId", "MedicamentoId", registrovisitum.MedicamentoId);
            ViewData["MedicoId"] = new SelectList(_context.Medicos, "MedicoId", "MedicoId", registrovisitum.MedicoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "PacienteId", "PacienteId", registrovisitum.PacienteId);
            return View(registrovisitum);
        }

        // GET: Registrovisitas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Registrovisita == null)
            {
                return NotFound();
            }

            var registrovisitum = await _context.Registrovisita
                .Include(r => r.Estado)
                .Include(r => r.Medicamento)
                .Include(r => r.Medico)
                .Include(r => r.Paciente)
                .FirstOrDefaultAsync(m => m.VisitaId == id);
            if (registrovisitum == null)
            {
                return NotFound();
            }

            return View(registrovisitum);
        }

        // POST: Registrovisitas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Registrovisita == null)
            {
                return Problem("Entity set 'sgmed_unapecContext.Registrovisita'  is null.");
            }
            var registrovisitum = await _context.Registrovisita.FindAsync(id);
            if (registrovisitum != null)
            {
                _context.Registrovisita.Remove(registrovisitum);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistrovisitumExists(int id)
        {
          return (_context.Registrovisita?.Any(e => e.VisitaId == id)).GetValueOrDefault();
        }
    }
}
