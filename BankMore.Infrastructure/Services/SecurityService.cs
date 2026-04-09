using BankMore.Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace BankMore.Infrastructure.Services
{
    public class SecurityService : ISecurityService
    {
        public string GerarHash(string senha, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var combinedPassword = string.Concat(senha, salt);
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedPassword));
                return Convert.ToBase64String(bytes);
            }
        }

        public bool VerificarSenha(string senhaDigitada, string senhaDoBanco, string saltDoBanco)
        {
            var hashDigitado = GerarHash(senhaDigitada, saltDoBanco);
            return hashDigitado == senhaDoBanco;
        }
    }
}