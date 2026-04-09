using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankMore.Domain.Entities
{
    public class ContaCorrente
    {
        public int Id { get; set; } 
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public int Ativo { get; set; }
        public string Senha { get; set; }
        public string Salt { get; set; }
        public decimal Saldo { get; set; }
        public long NumeroDaConta { get; set; }

    }
}
