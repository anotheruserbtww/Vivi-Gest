using System;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;

namespace ViviGest.Utilities
{
    public class EncryptUtility
    {
        // Método para encriptar la contraseña usando bcrypt
        public static string HashPassword(string password)
        {
            
                // Se genera el hash con un sal automático
                return BCrypt.Net.BCrypt.HashPassword(password);
            
        }

        // Método para verificar una contraseña en comparación con un hash almacenado
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Se compara la contraseña con el hash
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
