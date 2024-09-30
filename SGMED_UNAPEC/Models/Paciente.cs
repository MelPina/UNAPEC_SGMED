using System;
using System.Collections.Generic;

namespace SGMED_UNAPEC.Models
{
    public partial class Paciente
    {
        public Paciente()
        {
            Registrovisita = new HashSet<Registrovisitum>();
        }

        public int PacienteId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Cedula { get; set; } = null!;
        public string NoCarnet { get; set; } = null!;
        public string TipoPaciente { get; set; } = null!;
        public int EstadoId { get; set; }

        public virtual Estado Estado { get; set; } = null!;
        public virtual ICollection<Registrovisitum> Registrovisita { get; set; }
    }
}
