using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankMore.Application.Commands
{
    public record CadastrarContaCommand(string Cpf, string Senha, string Nome) : IRequest<int>;
}