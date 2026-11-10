
using System;
using System.Data;
using System.Data.SqlClient;

namespace SistemaBiblioteca.Data
{
    public class PrestamoRepository
    {
        private readonly ConexionBD conexionBD = new ConexionBD();

        public DataTable ObtenerLectores()
        {
            return Consultar(@"
                SELECT
                    Num_Tarjeta,
                    Nombre_Lector
                FROM dbo.Lector
                ORDER BY Nombre_Lector");
        }

        public DataTable ObtenerLibros()
        {
            return Consultar(@"
                SELECT
                    Id_Libro,
                    Titulo
                FROM dbo.Libro
                ORDER BY Titulo");
        }

        public DataTable ObtenerSucursales()
        {
            return Consultar(@"
                SELECT
                    Id_Sucursal,
                    Nombre_Sucursal
                FROM dbo.[Sucursal_Biblioteca]
                ORDER BY Nombre_Sucursal");
        }

        public DataTable ObtenerPrestamos()
        {
            return Consultar(@"
                SELECT
                    P.Id_Prestamo,
                    L.Nombre_Lector AS Lector,
                    B.Titulo AS Libro,
                    S.Nombre_Sucursal AS Sucursal,
                    P.Fecha_Salida,
                    P.Fecha_Devol
                FROM dbo.Prestamos P
                INNER JOIN dbo.Lector L
                    ON P.Num_Tarjeta = L.Num_Tarjeta
                INNER JOIN dbo.Libro B
                    ON P.Id_Libro = B.Id_Libro
                INNER JOIN dbo.[Sucursal_Biblioteca] S
                    ON P.Id_Sucursal = S.Id_Sucursal
                ORDER BY P.Fecha_Salida DESC");
        }

        public DataTable BuscarPrestamos(string criterio, string texto)
        {
            string columna;

            switch (criterio)
            {
                case "Usuario":
                    columna = "L.Nombre_Lector";
                    break;

                case "Libro":
                    columna = "B.Titulo";
                    break;

                default:
                    throw new Exception("Criterio de búsqueda no válido.");
            }

            string sql = @"
                SELECT
                    P.Id_Prestamo,
                    L.Nombre_Lector AS Lector,
                    B.Titulo AS Libro,
                    S.Nombre_Sucursal AS Sucursal,
                    P.Fecha_Salida,
                    P.Fecha_Devol
                FROM dbo.Prestamos P
                INNER JOIN dbo.Lector L
                    ON P.Num_Tarjeta = L.Num_Tarjeta
                INNER JOIN dbo.Libro B
                    ON P.Id_Libro = B.Id_Libro
                INNER JOIN dbo.[Sucursal_Biblioteca] S
                    ON P.Id_Sucursal = S.Id_Sucursal
                WHERE " + columna + @" LIKE @Texto
                ORDER BY P.Fecha_Salida DESC";

            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@Texto", SqlDbType.NVarChar, 200)
                    .Value = "%" + texto + "%";

                DataTable tabla = new DataTable();

                using (SqlDataAdapter adaptador =
                    new SqlDataAdapter(comando))
                {
                    adaptador.Fill(tabla);
                }

                return tabla;
            }
        }

        public void RegistrarPrestamo(
            int idLibro,
            int idSucursal,
            string numTarjeta,
            DateTime fechaSalida,
            DateTime fechaDevolucion)
        {
            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                string sql = @"
                    INSERT INTO dbo.Prestamos
                    (
                        Id_Libro,
                        Id_Sucursal,
                        Num_Tarjeta,
                        Fecha_Salida,
                        Fecha_Devol
                    )
                    VALUES
                    (
                        @IdLibro,
                        @IdSucursal,
                        @NumTarjeta,
                        @FechaSalida,
                        @FechaDevolucion
                    )";

                using (SqlCommand comando = new SqlCommand(sql, conexion))
                {
                    comando.Parameters.Add("@IdLibro", SqlDbType.Int)
                        .Value = idLibro;

                    comando.Parameters.Add("@IdSucursal", SqlDbType.Int)
                        .Value = idSucursal;

                    comando.Parameters.Add("@NumTarjeta", SqlDbType.Char, 5)
    .Value = numTarjeta;

                    comando.Parameters.Add("@FechaSalida", SqlDbType.DateTime)
                        .Value = fechaSalida;

                    comando.Parameters.Add("@FechaDevolucion", SqlDbType.DateTime)
                        .Value = fechaDevolucion;

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        private DataTable Consultar(string sql)
        {
            DataTable tabla = new DataTable();

            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            using (SqlDataAdapter adaptador =
                new SqlDataAdapter(sql, conexion))
            {
                adaptador.Fill(tabla);
            }

            return tabla;
        }
    }
}
