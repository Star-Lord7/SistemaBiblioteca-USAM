
using System;
using System.Collections.Generic;
using System.Data;
using SistemaBiblioteca.Data;
using SistemaBiblioteca.Models;

namespace SistemaBiblioteca.Services
{
    public class LibroService
    {
        private readonly LibroRepository libroRepository =
            new LibroRepository();

        public DataTable ListarDetalleLibros()
        {
            return libroRepository.ObtenerDetalleLibros();
        }

        public List<Autor> ListarAutores()
        {
            return libroRepository.ObtenerAutores();
        }

        public List<Editorial> ListarEditoriales()
        {
            return libroRepository.ObtenerEditoriales();
        }

        public List<Categoria> ListarCategorias()
        {
            return libroRepository.ObtenerCategorias();
        }

        public int ObtenerTotalLibros()
        {
            return libroRepository.ContarLibros();
        }

        public void GuardarLibro(Libro libro, int idAutor)
        {
            ValidarLibro(libro, idAutor);
            libroRepository.InsertarLibro(libro, idAutor);
        }

        public void EditarLibro(Libro libro, int idAutor)
        {
            if (libro.Id_Libro <= 0)
                throw new Exception("Seleccione un libro.");

            ValidarLibro(libro, idAutor);
            libroRepository.ActualizarLibro(libro, idAutor);
        }

        public void BorrarLibro(int idLibro)
        {
            if (idLibro <= 0)
                throw new Exception("Seleccione un libro.");

            libroRepository.EliminarLibro(idLibro);
        }

        private void ValidarLibro(Libro libro, int idAutor)
        {
            if (string.IsNullOrWhiteSpace(libro.Titulo))
                throw new Exception("Ingrese el título.");

            if (string.IsNullOrWhiteSpace(libro.Cod_Editorial))
                throw new Exception("Seleccione una editorial.");

            if (string.IsNullOrWhiteSpace(libro.IdCategoria))
                throw new Exception("Seleccione una categoría.");

            if (idAutor <= 0)
                throw new Exception("Seleccione un autor.");
        }
    }
}
