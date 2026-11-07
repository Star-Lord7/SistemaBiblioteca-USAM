using SistemaBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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


        public List<Rol> ObtenerRoles()
        {
            List<Rol> roles = new List<Rol>();

            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                string sql = "SELECT IdRol, NombreRol FROM dbo.Roles ORDER BY IdRol";

                using (SqlCommand comando = new SqlCommand(sql, conexion))
                {
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            roles.Add(new Rol
                            {
                                IdRol = Convert.ToInt32(lector["IdRol"]),
                                NombreRol = lector["NombreRol"].ToString()
                            });
                        }
                    }
                }
            }

            return roles;
        }

        public DataTable ObtenerUsuarios()
        {
            DataTable tabla = new DataTable();

            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                string sql = @"
            SELECT
                U.IdUsuario,
                U.Nombre,
                U.Apellido,
                U.Correo,
                R.NombreRol AS Rol,
                U.Estado,
                U.FechaRegistro
            FROM dbo.Usuarios U
            INNER JOIN dbo.Roles R ON U.IdRol = R.IdRol
            ORDER BY U.IdUsuario DESC";

                using (SqlDataAdapter adaptador =
                    new SqlDataAdapter(sql, conexion))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }

        public bool ExisteCorreo(string correo)
        {
            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                string sql = @"
            SELECT COUNT(*)
            FROM dbo.Usuarios
            WHERE Correo = @Correo";

                using (SqlCommand comando = new SqlCommand(sql, conexion))
                {
                    comando.Parameters.Add("@Correo", SqlDbType.NVarChar, 100)
                        .Value = correo;

                    conexion.Open();

                    return Convert.ToInt32(comando.ExecuteScalar()) > 0;
                }
            }
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

        public void ActualizarUsuario(Usuario usuario)
        {
            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                string sql = @"
            UPDATE dbo.Usuarios
            SET Nombre = @Nombre,
                Apellido = @Apellido,
                Correo = @Correo,
                IdRol = @IdRol
            WHERE IdUsuario = @IdUsuario";

                using (SqlCommand comando = new SqlCommand(sql, conexion))
                {
                    comando.Parameters.Add("@IdUsuario", SqlDbType.Int)
                        .Value = usuario.IdUsuario;

                    comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100)
                        .Value = usuario.Nombre;

                    comando.Parameters.Add("@Apellido", SqlDbType.NVarChar, 100)
                        .Value = usuario.Apellido;

                    comando.Parameters.Add("@Correo", SqlDbType.NVarChar, 100)
                        .Value = usuario.Correo;

                    comando.Parameters.Add("@IdRol", SqlDbType.Int)
                        .Value = usuario.IdRol;

                    conexion.Open();

                    if (comando.ExecuteNonQuery() == 0)
                        throw new Exception("El usuario ya no existe.");
                }
            }
        }

        public void EliminarUsuario(int idUsuario)
        {
            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                string sql = @"
            DELETE FROM dbo.Usuarios
            WHERE IdUsuario = @IdUsuario";

                using (SqlCommand comando = new SqlCommand(sql, conexion))
                {
                    comando.Parameters.Add("@IdUsuario", SqlDbType.Int)
                        .Value = idUsuario;

                    conexion.Open();

                    if (comando.ExecuteNonQuery() == 0)
                        throw new Exception("El usuario ya no existe.");
                }
            }
        }
    }
}