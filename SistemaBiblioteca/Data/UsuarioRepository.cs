using System;
using System.Data;
using System.Data.SqlClient;
using SistemaBiblioteca.Models;

namespace SistemaBiblioteca.Data
{
    public class UsuarioRepository
    {
        private readonly ConexionBD conexionBD = new ConexionBD();

        public Usuario ObtenerPorCorreo(string correo)
        {
            Usuario usuario = null;

            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                string consulta = @"
                    SELECT
                        U.IdUsuario,
                        U.Nombre,
                        U.Apellido,
                        U.Correo,
                        U.PasswordHash,
                        U.IdRol,
                        U.Estado,
                        R.NombreRol
                    FROM dbo.Usuarios U
                    INNER JOIN dbo.Roles R
                        ON U.IdRol = R.IdRol
                    WHERE U.Correo = @Correo";

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.Add(
                        "@Correo", SqlDbType.NVarChar, 100
                    ).Value = correo;

                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            usuario = new Usuario
                            {
                                IdUsuario = Convert.ToInt32(lector["IdUsuario"]),
                                Nombre = lector["Nombre"].ToString(),
                                Apellido = lector["Apellido"].ToString(),
                                Correo = lector["Correo"].ToString(),
                                PasswordHash = lector["PasswordHash"].ToString(),
                                IdRol = Convert.ToInt32(lector["IdRol"]),
                                NombreRol = lector["NombreRol"].ToString(),
                                Estado = Convert.ToBoolean(lector["Estado"])
                            };
                        }
                    }
                }
            }

            return usuario;
        }

        public void RegistrarUsuario(Usuario usuario)
        {
            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                string consulta = @"
                    INSERT INTO dbo.Usuarios
                    (
                        Nombre,
                        Apellido,
                        Correo,
                        PasswordHash,
                        IdRol
                    )
                    VALUES
                    (
                        @Nombre,
                        @Apellido,
                        @Correo,
                        @PasswordHash,
                        @IdRol
                    )";

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    comando.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    comando.Parameters.AddWithValue("@Correo", usuario.Correo);
                    comando.Parameters.AddWithValue(
                        "@PasswordHash", usuario.PasswordHash);
                    comando.Parameters.AddWithValue("@IdRol", usuario.IdRol);

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }
    }
}