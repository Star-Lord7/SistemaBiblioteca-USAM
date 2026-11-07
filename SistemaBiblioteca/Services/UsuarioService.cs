
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using SistemaBiblioteca.Data;
using SistemaBiblioteca.Models;

namespace SistemaBiblioteca.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepository repositorio =
            new UsuarioRepository();

        public List<Rol> ListarRoles()
        {
            return repositorio.ObtenerRoles();
        }

        public DataTable ListarUsuarios()
        {
            return repositorio.ObtenerUsuarios();
        }

        public void GuardarUsuario(
            string nombre,
            string apellido,
            string correo,
            string password,
            int idRol)
        {
            nombre = nombre.Trim();
            apellido = apellido.Trim();
            correo = correo.Trim();

            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(apellido) ||
                string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Complete todos los campos.");
            }

            if (nombre.Length > 100 || apellido.Length > 100 ||
                correo.Length > 100)
            {
                throw new Exception("Uno de los campos supera la longitud permitida.");
            }

            try
            {
                var direccion = new MailAddress(correo);

                if (direccion.Address != correo)
                    throw new Exception();
            }
            catch
            {
                throw new Exception("Ingrese un correo electrónico válido.");
            }

            if (password.Length < 8)
                throw new Exception("La contraseña debe tener al menos 8 caracteres.");

            if (idRol <= 0)
                throw new Exception("Seleccione un rol.");

            if (!repositorio.ObtenerRoles().Exists(r => r.IdRol == idRol))
                throw new Exception("El rol seleccionado no existe.");

            if (repositorio.ExisteCorreo(correo))
                throw new Exception("Ya existe un usuario con ese correo.");

            Usuario usuario = new Usuario
            {
                Nombre = nombre,
                Apellido = apellido,
                Correo = correo,
                PasswordHash = PasswordService.GenerarHash(password),
                IdRol = idRol,
                Estado = true
            };

            repositorio.RegistrarUsuario(usuario);
        }

        public void ActualizarUsuario(
            int idUsuario,
            string nombre,
            string apellido,
            string correo,
            int idRol)
        {
            nombre = nombre.Trim();
            apellido = apellido.Trim();
            correo = correo.Trim();

            if (idUsuario <= 0)
                throw new Exception("Seleccione un usuario.");

            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(apellido) ||
                string.IsNullOrWhiteSpace(correo))
            {
                throw new Exception("Complete todos los campos.");
            }

            if (nombre.Length > 100 || apellido.Length > 100 ||
                correo.Length > 100)
            {
                throw new Exception("Uno de los campos supera la longitud permitida.");
            }

            try
            {
                var direccion = new System.Net.Mail.MailAddress(correo);

                if (direccion.Address != correo)
                    throw new Exception();
            }
            catch
            {
                throw new Exception("Ingrese un correo electrónico válido.");
            }

            if (!repositorio.ObtenerRoles().Exists(r => r.IdRol == idRol))
                throw new Exception("Seleccione un rol válido.");

            Usuario usuario = new Usuario
            {
                IdUsuario = idUsuario,
                Nombre = nombre,
                Apellido = apellido,
                Correo = correo,
                IdRol = idRol
            };

            repositorio.ActualizarUsuario(usuario);
        }

        public void EliminarUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new Exception("Seleccione un usuario.");

            repositorio.EliminarUsuario(idUsuario);
        }

    }
}
