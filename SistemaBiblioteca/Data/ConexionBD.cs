using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using System.Data.SqlClient;

namespace SistemaBiblioteca.Data
{
    public class ConexionBD
    {
        private readonly string cadenaConexion;

        public ConexionBD()
        {
            cadenaConexion = ConfigurationManager
                .ConnectionStrings["Biblioteca"]
                .ConnectionString;
        }

        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadenaConexion);
        }
    }
}

