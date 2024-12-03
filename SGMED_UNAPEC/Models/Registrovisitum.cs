using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SGMED_UNAPEC.Models
{
    public partial class Registrovisitum
    {
        //public int VisitaId { get; set; }
        //public int MedicoId { get; set; }
        //public int PacienteId { get; set; }
        //public DateOnly FechaVisita { get; set; }
        //public TimeOnly HoraVisita { get; set; }
        //public string Sintomas { get; set; } = null!;
        //public int MedicamentoId { get; set; }
        //public string? Recomendaciones { get; set; }
        //public int EstadoId { get; set; }

        //public virtual Estado Estado { get; set; } = null!;
        //public virtual Medicamento Medicamento { get; set; } = null!;
        //public virtual Medico Medico { get; set; } = null!;
        //public virtual Paciente Paciente { get; set; } = null!;
        public int VisitaId { get; set; }

        [Required]
        public int MedicoId { get; set; }

        [Required]
        public int PacienteId { get; set; }

        [Required]
        public DateOnly FechaVisita { get; set; }

        [Required]
        public TimeOnly HoraVisita { get; set; }

        [Required]
        [StringLength(500)]
        public string Sintomas { get; set; } = null!;

        [Required]
        public int MedicamentoId { get; set; }

        public string? Recomendaciones { get; set; }

        [Required]
        public int EstadoId { get; set; }

        public virtual Estado Estado { get; set; } = null!;
        public virtual Medicamento Medicamento { get; set; } = null!;
        public virtual Medico Medico { get; set; } = null!;
        public virtual Paciente Paciente { get; set; } = null!;
    }
}
