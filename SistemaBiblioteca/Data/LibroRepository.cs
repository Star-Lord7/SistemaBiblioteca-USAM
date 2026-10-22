
using SistemaBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SistemaBiblioteca.Data
{
    public class LibroRepository
    {
        private readonly ConexionBD conexionBD = new ConexionBD();

        // Método para obtener todos los libros
        public DataTable ObtenerDetalleLibros()
        {
            DataTable tabla = new DataTable();

            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                string sql = @"
            SELECT
                L.Id_Libro,
                L.Titulo,
                L.Cod_Editorial,
                L.IdCategoria,
                E.Nombre_Ed AS Editorial,
                C.Categoria AS Categoria,
                A.Id_Autor,
                A.Nombre_Autor AS Autor
            FROM dbo.Libro L
            LEFT JOIN dbo.Editorial E
                ON L.Cod_Editorial = E.Cod_Ed
            LEFT JOIN dbo.Categoria C
                ON L.IdCategoria = C.IdCategoria
            LEFT JOIN dbo.Autores_Libros AL
                ON L.Id_Libro = AL.Id_Libro
            LEFT JOIN dbo.Autor A
                ON AL.Id_Autor = A.Id_Autor
            ORDER BY L.Id_Libro";

                using (SqlDataAdapter adaptador =
                    new SqlDataAdapter(sql, conexion))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }


        public void InsertarLibro(Libro libro, int idAutor)
        {
            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                conexion.Open();

                using (SqlTransaction transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        string sqlLibro = @"
                    INSERT INTO dbo.Libro
                        (Titulo, Cod_Editorial, IdCategoria)
                    OUTPUT INSERTED.Id_Libro
                    VALUES
                        (@Titulo, @CodEditorial, @IdCategoria)";

                        int idLibro;

                        using (SqlCommand cmd =
                            new SqlCommand(sqlLibro, conexion, transaccion))
                        {
                            cmd.Parameters.AddWithValue(
                                "@Titulo", libro.Titulo);

                            cmd.Parameters.AddWithValue(
                                "@CodEditorial", libro.Cod_Editorial);

                            cmd.Parameters.AddWithValue(
                                "@IdCategoria", libro.IdCategoria);

                            idLibro = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        string sqlAutor = @"
                    INSERT INTO dbo.Autores_Libros
                        (Id_Libro, Id_Autor)
                    VALUES
                        (@IdLibro, @IdAutor)";

                        using (SqlCommand cmd =
                            new SqlCommand(sqlAutor, conexion, transaccion))
                        {
                            cmd.Parameters.AddWithValue("@IdLibro", idLibro);
                            cmd.Parameters.AddWithValue("@IdAutor", idAutor);

                            cmd.ExecuteNonQuery();
                        }

                        transaccion.Commit();
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }


        public void ActualizarLibro(Libro libro, int idAutor)
        {
            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                conexion.Open();

                using (SqlTransaction transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        string sqlLibro = @"
                    UPDATE dbo.Libro
                    SET Titulo = @Titulo,
                        Cod_Editorial = @CodEditorial,
                        IdCategoria = @IdCategoria
                    WHERE Id_Libro = @IdLibro";

                        using (SqlCommand cmd =
                            new SqlCommand(sqlLibro, conexion, transaccion))
                        {
                            cmd.Parameters.AddWithValue(
                                "@IdLibro", libro.Id_Libro);
                            cmd.Parameters.AddWithValue(
                                "@Titulo", libro.Titulo);
                            cmd.Parameters.AddWithValue(
                                "@CodEditorial", libro.Cod_Editorial);
                            cmd.Parameters.AddWithValue(
                                "@IdCategoria", libro.IdCategoria);

                            if (cmd.ExecuteNonQuery() == 0)
                                throw new Exception("El libro no existe.");
                        }

                        string sqlEliminarAutores = @"
                    DELETE FROM dbo.Autores_Libros
                    WHERE Id_Libro = @IdLibro";

                        using (SqlCommand cmd =
                            new SqlCommand(sqlEliminarAutores, conexion, transaccion))
                        {
                            cmd.Parameters.AddWithValue(
                                "@IdLibro", libro.Id_Libro);

                            cmd.ExecuteNonQuery();
                        }

                        string sqlInsertarAutor = @"
                    INSERT INTO dbo.Autores_Libros
                        (Id_Libro, Id_Autor)
                    VALUES (@IdLibro, @IdAutor)";

                        using (SqlCommand cmd =
                            new SqlCommand(sqlInsertarAutor, conexion, transaccion))
                        {
                            cmd.Parameters.AddWithValue(
                                "@IdLibro", libro.Id_Libro);
                            cmd.Parameters.AddWithValue(
                                "@IdAutor", idAutor);

                            cmd.ExecuteNonQuery();
                        }

                        transaccion.Commit();
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }


        public void EliminarLibro(int idLibro)
        {
            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                conexion.Open();

                using (SqlTransaction transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        string sqlAutores = @"
                    DELETE FROM dbo.Autores_Libros
                    WHERE Id_Libro = @IdLibro";

                        using (SqlCommand cmd =
                            new SqlCommand(sqlAutores, conexion, transaccion))
                        {
                            cmd.Parameters.AddWithValue("@IdLibro", idLibro);
                            cmd.ExecuteNonQuery();
                        }

                        string sqlLibro = @"
                    DELETE FROM dbo.Libro
                    WHERE Id_Libro = @IdLibro";

                        using (SqlCommand cmd =
                            new SqlCommand(sqlLibro, conexion, transaccion))
                        {
                            cmd.Parameters.AddWithValue("@IdLibro", idLibro);

                            if (cmd.ExecuteNonQuery() == 0)
                                throw new Exception("El libro no existe.");
                        }

                        transaccion.Commit();
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }


        // Método para contar el número total de libros
        public int ContarLibros()
        {
            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                string consulta = "SELECT COUNT(*) FROM dbo.Libro";

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    conexion.Open();

                    int totalLibros = Convert.ToInt32(comando.ExecuteScalar());

                    return totalLibros;
                }
            }
        }

        public List<Autor> ObtenerAutores()
        {
            List<Autor> autores = new List<Autor>();

            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                string sql = "SELECT Id_Autor, Nombre_Autor FROM dbo.Autor";

                using (SqlCommand cmd = new SqlCommand(sql, conexion))
                {
                    conexion.Open();

                    using (SqlDataReader lector = cmd.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            autores.Add(new Autor
                            {
                                Id_Autor = Convert.ToInt32(lector["Id_Autor"]),
                                Nombre_Autor = lector["Nombre_Autor"].ToString()
                            });
                        }
                    }
                }
            }

            return autores;
        }

        public List<Editorial> ObtenerEditoriales()
        {
            List<Editorial> editoriales = new List<Editorial>();

            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                string sql = "SELECT Cod_Ed, Nombre_Ed FROM dbo.Editorial";

                using (SqlCommand cmd = new SqlCommand(sql, conexion))
                {
                    conexion.Open();

                    using (SqlDataReader lector = cmd.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            editoriales.Add(new Editorial
                            {
                                Cod_Ed = lector["Cod_Ed"].ToString(),
                                Nombre_Ed = lector["Nombre_Ed"].ToString()
                            });
                        }
                    }
                }
            }

            return editoriales;
        }

        public List<Categoria> ObtenerCategorias()
        {
            List<Categoria> categorias = new List<Categoria>();

            using (SqlConnection conexion = conexionBD.ObtenerConexion())
            {
                string sql = "SELECT IdCategoria, Categoria FROM dbo.Categoria";

                using (SqlCommand cmd = new SqlCommand(sql, conexion))
                {
                    conexion.Open();

                    using (SqlDataReader lector = cmd.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            categorias.Add(new Categoria
                            {
                                IdCategoria = lector["IdCategoria"].ToString(),
                                NombreCategoria = lector["Categoria"].ToString()
                            });
                        }
                    }
                }
            }

            return categorias;
        }


    }
}
