using System;
using System.Collections.Generic;

namespace SGMED_UNAPEC.Models
{
    public partial class Medicamento
    {
        public Medicamento()
        {
            Registrovisita = new HashSet<Registrovisitum>();
        }

        public int MedicamentoId { get; set; }
        public string Descripcion { get; set; } = null!;
        public int TipoFarmacoId { get; set; }
        public int MarcaId { get; set; }
        public int UbicacionId { get; set; }
        public string Dosis { get; set; } = null!;
        public int EstadoId { get; set; }

        public virtual Estado Estado { get; set; } = null!;
        public virtual Marca Marca { get; set; } = null!;
        public virtual Tipofarmaco TipoFarmaco { get; set; } = null!;
        public virtual Ubicacion Ubicacion { get; set; } = null!;
        public virtual ICollection<Registrovisitum> Registrovisita { get; set; }
    }
}
