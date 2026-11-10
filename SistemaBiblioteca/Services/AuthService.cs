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
        private readonly UsuarioRepository usuarioRepository = new UsuarioRepository();

        public Usuario IniciarSesion(string correo, string password)
        {
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Debe ingresar su correo y contraseña.");
            }

            // Bypass directo para el administrador con contraseña 12345
            if (correo.Trim().ToLower() == "admin@biblioteca.com" && password == "12345")
            {
                Usuario adminUsuario = usuarioRepository.ObtenerPorCorreo(correo.Trim());
                if (adminUsuario != null)
                {
                    adminUsuario.PasswordHash = null;
                    return adminUsuario;
                }
            }

            Usuario usuario = usuarioRepository.ObtenerPorCorreo(correo.Trim());

            if (usuario == null)
            {
                return null;
            }

            if (!PasswordService.VerificarPassword(password, usuario.PasswordHash))
            {
                return null;
            }

            if (!usuario.Estado)
            {
                throw new Exception("La cuenta se encuentra deshabilitada.");
            }

            usuario.PasswordHash = null;

            return usuario;
        }
    }
}