
using SGMED_UNAPEC.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore; 


namespace SGMED_UNAPEC.Data
{
    public class DA_Usuario
    {
        private readonly sgmed_unapecContext _dbContext; 



        public DA_Usuario(sgmed_unapecContext dbContext)
        {
            _dbContext = dbContext;
        }



        public List<Usuario> ListaUsuario()
        {
            // Recupera la lista de usuarios desde la base de datos usando Entity Framework
            return _dbContext.Usuarios.ToList();
        }



        public Usuario ValidarUsuario(string _user, string _clave)
        {
            // Busca al usuario en la base de datos
            return _dbContext.Usuarios.SingleOrDefault(item => item.Username == _user && item.Password == _clave && item.EstadoId == 1);
        }

        
    }
}

