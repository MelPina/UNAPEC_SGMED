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
    public class MedicamentosController : Controller
    {
        private readonly sgmed_unapecContext _context;

        public MedicamentosController(sgmed_unapecContext context)
        {
            _context = context;
        }

        // GET: Medicamentos
        public async Task<IActionResult> Index()
        {
            var sgmed_unapecContext = _context.Medicamentos.Include(m => m.Estado).Include(m => m.Marca).Include(m => m.TipoFarmaco).Include(m => m.Ubicacion);
            return View(await sgmed_unapecContext.ToListAsync());
        }

        [HttpPost]
        public JsonResult ObtenerDatosMedicamentos()
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

                IQueryable<Medicamento> queryTbMedicamento = _context.Medicamentos
                    .Include(m => m.Estado)
                    .Include(m => m.Marca)
                    .Include(m => m.TipoFarmaco)
                    .Include(m => m.Ubicacion);

                // Filtro de búsqueda
                if (!string.IsNullOrEmpty(ValorBuscado))
                {
                    queryTbMedicamento = queryTbMedicamento.Where(m =>
                        m.MedicamentoId.ToString().Contains(ValorBuscado) ||
                        m.Descripcion.Contains(ValorBuscado) ||
                        m.Estado.Descripcion.Contains(ValorBuscado) ||
                        m.Marca.Descripcion.Contains(ValorBuscado) ||
                        m.TipoFarmaco.Descripcion.Contains(ValorBuscado) ||
                        m.Ubicacion.Descripcion.Contains(ValorBuscado));
                }

                // Total de registros antes de filtrar
                int TotalRegistros = _context.Medicamentos.Count();

                // Total de registros filtrados
                int TotalRegistrosFiltrados = queryTbMedicamento.Count();

                // Obtener registros paginados
                var listaMedicamentos = queryTbMedicamento
                    .Skip(OmitirRegistros)
                    .Take(CantidadRegistros)
                    .Select(m => new
                    {
                        m.MedicamentoId,
                        m.Descripcion,
                        Marca = m.Marca.Descripcion,
                        TipoFarmaco = m.TipoFarmaco.Descripcion,
                        Ubicacion = m.Ubicacion.Descripcion,
                        Estado = m.Estado.Descripcion,
                        m.Dosis
                    })
                    .ToList();

                // Devolver respuesta JSON esperada por DataTables
                return Json(new
                {
                    draw = NroPeticion,
                    recordsTotal = TotalRegistros,
                    recordsFiltered = TotalRegistrosFiltrados,
                    data = listaMedicamentos // Esta propiedad debe llamarse "data"
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


        // GET: Medicamentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Medicamentos == null)
            {
                return NotFound();
            }

            var medicamento = await _context.Medicamentos
                .Include(m => m.Estado)
                .Include(m => m.Marca)
                .Include(m => m.TipoFarmaco)
                .Include(m => m.Ubicacion)
                .FirstOrDefaultAsync(m => m.MedicamentoId == id);
            if (medicamento == null)
            {
                return NotFound();
            }

            return View(medicamento);
        }

        // GET: Medicamentos/Create
        public IActionResult Create()
        {
            ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "EstadoId");
            ViewData["MarcaId"] = new SelectList(_context.Marcas, "MarcaId", "MarcaId");
            ViewData["TipoFarmacoId"] = new SelectList(_context.Tipofarmacos, "TipoFarmacoId", "TipoFarmacoId");
            ViewData["UbicacionId"] = new SelectList(_context.Ubicacions, "UbicacionId", "UbicacionId");
            return View();
        }

        // POST: Medicamentos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MedicamentoId,Descripcion,TipoFarmacoId,MarcaId,UbicacionId,Dosis,EstadoId")] Medicamento medicamento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "EstadoId", medicamento.EstadoId);
            ViewData["MarcaId"] = new SelectList(_context.Marcas, "MarcaId", "MarcaId", medicamento.MarcaId);
            ViewData["TipoFarmacoId"] = new SelectList(_context.Tipofarmacos, "TipoFarmacoId", "TipoFarmacoId", medicamento.TipoFarmacoId);
            ViewData["UbicacionId"] = new SelectList(_context.Ubicacions, "UbicacionId", "UbicacionId", medicamento.UbicacionId);
            return View(medicamento);
        }

        // GET: Medicamentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Medicamentos == null)
            {
                return NotFound();
            }

            var medicamento = await _context.Medicamentos.FindAsync(id);
            if (medicamento == null)
            {
                return NotFound();
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "EstadoId", medicamento.EstadoId);
            ViewData["MarcaId"] = new SelectList(_context.Marcas, "MarcaId", "MarcaId", medicamento.MarcaId);
            ViewData["TipoFarmacoId"] = new SelectList(_context.Tipofarmacos, "TipoFarmacoId", "TipoFarmacoId", medicamento.TipoFarmacoId);
            ViewData["UbicacionId"] = new SelectList(_context.Ubicacions, "UbicacionId", "UbicacionId", medicamento.UbicacionId);
            return View(medicamento);
        }

        // POST: Medicamentos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MedicamentoId,Descripcion,TipoFarmacoId,MarcaId,UbicacionId,Dosis,EstadoId")] Medicamento medicamento)
        {
            if (id != medicamento.MedicamentoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicamentoExists(medicamento.MedicamentoId))
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
            ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "EstadoId", medicamento.EstadoId);
            ViewData["MarcaId"] = new SelectList(_context.Marcas, "MarcaId", "MarcaId", medicamento.MarcaId);
            ViewData["TipoFarmacoId"] = new SelectList(_context.Tipofarmacos, "TipoFarmacoId", "TipoFarmacoId", medicamento.TipoFarmacoId);
            ViewData["UbicacionId"] = new SelectList(_context.Ubicacions, "UbicacionId", "UbicacionId", medicamento.UbicacionId);
            return View(medicamento);
        }

        // GET: Medicamentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Medicamentos == null)
            {
                return NotFound();
            }

            var medicamento = await _context.Medicamentos
                .Include(m => m.Estado)
                .Include(m => m.Marca)
                .Include(m => m.TipoFarmaco)
                .Include(m => m.Ubicacion)
                .FirstOrDefaultAsync(m => m.MedicamentoId == id);
            if (medicamento == null)
            {
                return NotFound();
            }

            return View(medicamento);
        }

        // POST: Medicamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Medicamentos == null)
            {
                return Problem("Entity set 'sgmed_unapecContext.Medicamentos'  is null.");
            }
            var medicamento = await _context.Medicamentos.FindAsync(id);
            if (medicamento != null)
            {
                _context.Medicamentos.Remove(medicamento);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicamentoExists(int id)
        {
          return (_context.Medicamentos?.Any(e => e.MedicamentoId == id)).GetValueOrDefault();
        }
    }
}
