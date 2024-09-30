using System;
using System.Collections.Generic;

namespace SGMED_UNAPEC.Models
{
    public partial class Tipofarmaco
    {
        public Tipofarmaco()
        {
            Medicamentos = new HashSet<Medicamento>();
        }

        public int TipoFarmacoId { get; set; }
        public string Descripcion { get; set; } = null!;
        public int EstadoId { get; set; }

        public virtual Estado Estado { get; set; } = null!;
        public virtual ICollection<Medicamento> Medicamentos { get; set; }
    }
}
