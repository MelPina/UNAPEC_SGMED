using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SGMED_UNAPEC.Models
{
    public partial class Usuario
    {
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string? Password { get; set; }

        public int RolId { get; set; }

        public int EstadoId { get; set; }

        public Estado Estado { get; set; } = null!;

        public Rol Rol { get; set; } = null!;
    }
}
