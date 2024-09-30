using System;
using System.Collections.Generic;

namespace SGMED_UNAPEC.Models
{
    public partial class Rol
    {
        public Rol()
        {
            Usuarios = new HashSet<Usuario>();
            Permisos = new HashSet<Permiso>();
        }

        public int RolId { get; set; }
        public string Descripcion { get; set; } = null!;

        public virtual ICollection<Usuario> Usuarios { get; set; }

        public virtual ICollection<Permiso> Permisos { get; set; }
    }
}
