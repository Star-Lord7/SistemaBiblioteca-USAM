using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaBiblioteca.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Correo { get; set; }

        public string PasswordHash { get; set; }

        public int IdRol { get; set; }

        public string NombreRol { get; set; }

        public bool Estado { get; set; }
    }
}
