using System;
using System.Collections.Generic;

namespace SGMED_UNAPEC.Models
{
    public partial class Permiso
    {
        public Permiso()
        {
            Rols = new HashSet<Rol>();
        }

        public int PermisoId { get; set; }
        public string Descripcion { get; set; } = null!;

        public virtual ICollection<Rol> Rols { get; set; }
    }
}
