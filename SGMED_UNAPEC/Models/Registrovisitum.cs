using System;
using System.Collections.Generic;

namespace SGMED_UNAPEC.Models
{
    public partial class Registrovisitum
    {
        public int VisitaId { get; set; }
        public int MedicoId { get; set; }
        public int PacienteId { get; set; }
        public DateOnly FechaVisita { get; set; }
        public TimeOnly HoraVisita { get; set; }
        public string Sintomas { get; set; } = null!;
        public int MedicamentoId { get; set; }
        public string? Recomendaciones { get; set; }
        public int EstadoId { get; set; }

        public virtual Estado Estado { get; set; } = null!;
        public virtual Medicamento Medicamento { get; set; } = null!;
        public virtual Medico Medico { get; set; } = null!;
        public virtual Paciente Paciente { get; set; } = null!;
    }
}
