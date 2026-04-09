using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankMore.Domain.Interfaces
{
    public interface ISecurityService
    {
        string GerarHash(string senha, string salt);
        bool VerificarSenha(string senhaDigitada, string senhaDoBanco, string saltDoBanco);
    }

}
