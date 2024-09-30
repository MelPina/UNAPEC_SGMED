using System;
using System.Collections.Generic;

namespace SGMED_UNAPEC.Models
{
    public partial class Ubicacion
    {
        public Ubicacion()
        {
            Medicamentos = new HashSet<Medicamento>();
        }

        public int UbicacionId { get; set; }
        public string Descripcion { get; set; } = null!;
        public string Estante { get; set; } = null!;
        public string Tramo { get; set; } = null!;
        public string Celda { get; set; } = null!;
        public int EstadoId { get; set; }

        public virtual Estado Estado { get; set; } = null!;
        public virtual ICollection<Medicamento> Medicamentos { get; set; }
    }
}
