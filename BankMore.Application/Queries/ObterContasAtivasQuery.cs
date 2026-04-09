using MediatR;
using BankMore.Domain.Entities;

namespace BankMore.Application.Queries
{
    public class ObterContasAtivasQuery : IRequest<IEnumerable<ContaCorrente>> { }
}