using System;
using System.Security.Cryptography;
using System.Text;

namespace CardStorageService.Utils
{
    public static class PasswordUtils
    {
        private const string _secretKey = "TestKey1!";

        public static (string passwordSalt, string passwordHash) CreatePasswordValues(string password)
        {
            // Генерация соли
            byte[] salt = new byte[32];
            RNGCryptoServiceProvider serviceProvider = new RNGCryptoServiceProvider();
            serviceProvider.GetBytes(salt);

            // Создание хэша
            string passwordSalt = Convert.ToBase64String(salt);
            string passwordHash = GetPasswordHash(password, passwordSalt);

            return (passwordSalt, passwordHash);
        }

        public static bool PasswordVerify(string password, string passwordSalt, string passwordHash)
        {
            var a = GetPasswordHash(password, passwordSalt);
            return GetPasswordHash(password, passwordSalt) == passwordHash;
        }

        private static string GetPasswordHash(string password, string passwordSalt)
        {
            // Усложняю пароль и генерирую соль
            password = $"{password}~{passwordSalt}~{_secretKey}";
            byte[] salt = Encoding.UTF8.GetBytes(password);

            // Создание хэша
            SHA512 sha512 = new SHA512Managed();
            byte[] hash = sha512.ComputeHash(salt);

            return Convert.ToBase64String(hash);
        }
    }
}
