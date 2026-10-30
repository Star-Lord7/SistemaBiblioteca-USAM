using System;
using System.Security.Cryptography;

namespace SistemaBiblioteca.Services
{
    public static class PasswordService
    {
        private const int Iteraciones = 600000;
        private const int TamanoSalt = 16;
        private const int TamanoHash = 32;

        // Generar hash para nuevos usuarios
        public static string GenerarHash(string password)
        {
            byte[] salt = new byte[TamanoSalt];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] hash;

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                Iteraciones,
                HashAlgorithmName.SHA256))
            {
                hash = pbkdf2.GetBytes(TamanoHash);
            }

            return "PBKDF2-SHA256$" +
                   Iteraciones + "$" +
                   Convert.ToBase64String(salt) + "$" +
                   Convert.ToBase64String(hash);
        }

        // Verificar contraseña durante el login
        public static bool VerificarPassword(
            string password,
            string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                return false;

            try
            {
                // Separar los componentes del hash
                string[] partes = passwordHash.Split('$');

                if (partes.Length != 4)
                    return false;

                // Comprobar algoritmo
                if (partes[0] != "PBKDF2-SHA256")
                    return false;

                // Obtener las iteraciones originales
                int iteraciones = int.Parse(partes[1]);

                if (iteraciones < 10000 || iteraciones > 2000000)
                    return false;

                // Recuperar salt y hash almacenados
                byte[] salt = Convert.FromBase64String(partes[2]);

                byte[] hashOriginal =
                    Convert.FromBase64String(partes[3]);

                if (salt.Length != TamanoSalt ||
                    hashOriginal.Length != TamanoHash)
                    return false;

                byte[] hashNuevo;

                // Generar un hash usando la contraseña ingresada
                using (var pbkdf2 = new Rfc2898DeriveBytes(
                    password,
                    salt,
                    iteraciones,
                    HashAlgorithmName.SHA256))
                {
                    hashNuevo = pbkdf2.GetBytes(hashOriginal.Length);
                }

                // Comparar los hashes sin terminar
                // anticipadamente por diferencias
                int diferencia = 0;

                for (int i = 0; i < hashOriginal.Length; i++)
                {
                    diferencia |= hashOriginal[i] ^ hashNuevo[i];
                }

                return diferencia == 0;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (OverflowException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}