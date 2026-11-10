
using System;
using System.Data;
using SistemaBiblioteca.Data;

namespace SistemaBiblioteca.Services
{
    public class PrestamoService
    {
        private readonly PrestamoRepository repositorio =
            new PrestamoRepository();

        public DataTable ObtenerLectores()
        {
            return repositorio.ObtenerLectores();
        }

        public DataTable ObtenerLibros()
        {
            return repositorio.ObtenerLibros();
        }

        public DataTable ObtenerSucursales()
        {
            return repositorio.ObtenerSucursales();
        }

        public DataTable ObtenerPrestamos()
        {
            return repositorio.ObtenerPrestamos();
        }

        public DataTable BuscarPrestamos(
            string criterio, string texto)
        {
            return repositorio.BuscarPrestamos(
                criterio, texto.Trim());
        }

        public void RegistrarPrestamo(
    int idLibro,
    int idSucursal,
    string numTarjeta,
    DateTime fechaSalida,
    DateTime fechaDevolucion)
        {
            if (fechaDevolucion.Date <= fechaSalida.Date)
            {
                throw new Exception(
                    "La fecha de devolución debe ser posterior a la fecha del préstamo.");
            }

            repositorio.RegistrarPrestamo(
                idLibro,
                idSucursal,
                numTarjeta,
                fechaSalida,
                fechaDevolucion);
        }
    }
}
