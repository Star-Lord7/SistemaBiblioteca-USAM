using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaBiblioteca.Models
{
    public class Libro
    {
        public int Id_Libro { get; set; }
        public string Titulo { get; set; }
        public string Cod_Editorial { get; set; }
        public string IdCategoria { get; set; }
    }
}
