using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankMore.Application.Commands
{
    public class EfetuarLoginCommand : IRequest<string>
    {
        public string CpfOuConta { get; set; }
        public string Senha { get; set; }
    }
}   