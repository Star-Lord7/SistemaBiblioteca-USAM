using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaBiblioteca.Data;
using SistemaBiblioteca.Models;

    namespace SistemaBiblioteca.Services
    {
        public class AuthService
        {
            private readonly UsuarioRepository usuarioRepository =
                new UsuarioRepository();

            public Usuario IniciarSesion(string correo, string password)
            {
                if (string.IsNullOrWhiteSpace(correo) ||
                    string.IsNullOrWhiteSpace(password))
                {
                    throw new Exception(
                        "Debe ingresar su correo y contraseña.");
                }

                Usuario usuario = usuarioRepository.ObtenerPorCorreo(
                    correo.Trim());

                if (usuario == null)
                {
                    return null;
                }

                if (!PasswordService.VerificarPassword(
                    password,
                    usuario.PasswordHash))
                {
                    return null;
                }

                if (!usuario.Estado)
                {
                    throw new Exception(
                        "La cuenta se encuentra deshabilitada.");
                }

                // Por ahora solamente permitimos administradores.
                if (usuario.NombreRol != "Administrador")
                {
                    throw new Exception(
                        "No tiene permisos para ingresar como administrador.");
                }

                // Evitar conservar el hash en el objeto de sesión.
                usuario.PasswordHash = null;

                return usuario;
            }
        }
}
