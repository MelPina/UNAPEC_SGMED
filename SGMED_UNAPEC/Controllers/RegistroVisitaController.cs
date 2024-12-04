using Microsoft.AspNetCore.Mvc;
using SGMED_UNAPEC.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SGMED_UNAPEC.Controllers
{
    public class RegistroVisitaController : Controller
    {
        private readonly sgmed_unapecContext _context;

        public RegistroVisitaController(sgmed_unapecContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Consultas()
        {
            ViewBag.Medicos = await _context.Medicos.ToListAsync();
            ViewBag.Pacientes = await _context.Pacientes.ToListAsync();
            return View();
        }

        [HttpGet]
        public JsonResult BuscarVisitas(int? medicoId, int? pacienteId, string? fechaVisita)
        {
            DateOnly? fecha = null;
            if (!string.IsNullOrEmpty(fechaVisita))
            {
                fecha = DateOnly.Parse(fechaVisita);
            }
            var visitas = _context.Registrovisita
                   .Include(r => r.Medico)
                   .Include(r => r.Paciente)
                   .Include(r => r.Estado)
                   .Where(r =>
                       (!medicoId.HasValue || r.MedicoId == medicoId) &&
                       (!pacienteId.HasValue || r.PacienteId == pacienteId) &&
                       (!fecha.HasValue || r.FechaVisita == fecha))
                   .Select(r => new
                   {
                       medico = r.Medico.Nombre,
                       paciente = r.Paciente.Nombre,
                       fechaVisita = r.FechaVisita.ToString("yyyy-MM-dd"),
                       horaVisita = r.HoraVisita.ToString(@"hh\:mm"),
                       r.Sintomas,
                       r.Recomendaciones,
                       estado = r.Estado.Descripcion
                   })
                   .ToList();

            return Json(visitas);
        }
    }
}
