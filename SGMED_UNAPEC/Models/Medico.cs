using System;
using System.Collections.Generic;

namespace SGMED_UNAPEC.Models
{
    public partial class Medico
    {
        public Medico()
        {
            Registrovisita = new HashSet<Registrovisitum>();
        }

        public int MedicoId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Cedula { get; set; } = null!;
        public string TandaLabor { get; set; } = null!;
        public string Especialidad { get; set; } = null!;
        public int EstadoId { get; set; }

        public virtual Estado Estado { get; set; } = null!;
        public virtual ICollection<Registrovisitum> Registrovisita { get; set; }
    }
}
