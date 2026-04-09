
using System;
using System.Security.Cryptography;
using System.Text;

namespace BankMore.Application.Services
{
    public static class SecurityService
    {
        public static string GerarHash(string senha, string salt)
        {
           
            using (var sha256 = SHA256.Create())
            {
                
                var senhaComSalt = senha + salt;
                var bytes = Encoding.UTF8.GetBytes(senhaComSalt);
                var hash = sha256.ComputeHash(bytes);

                
                return Convert.ToBase64String(hash);
            }
        }
    }
}