using System;
using System.Collections.Generic;

namespace SGMED_UNAPEC.Models
{
    public partial class Estado
    {
        public Estado()
        {
            Marcas = new HashSet<Marca>();
            Medicamentos = new HashSet<Medicamento>();
            Medicos = new HashSet<Medico>();
            Pacientes = new HashSet<Paciente>();
            Registrovisita = new HashSet<Registrovisitum>();
            Tipofarmacos = new HashSet<Tipofarmaco>();
            Ubicacions = new HashSet<Ubicacion>();
            Usuarios = new HashSet<Usuario>();
        }

        public int EstadoId { get; set; }
        public string Descripcion { get; set; } = null!;

        public virtual ICollection<Marca> Marcas { get; set; }
        public virtual ICollection<Medicamento> Medicamentos { get; set; }
        public virtual ICollection<Medico> Medicos { get; set; }
        public virtual ICollection<Paciente> Pacientes { get; set; }
        public virtual ICollection<Registrovisitum> Registrovisita { get; set; }
        public virtual ICollection<Tipofarmaco> Tipofarmacos { get; set; }
        public virtual ICollection<Ubicacion> Ubicacions { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
