using BankMore.Domain.Entities;
using MediatR;

namespace BankMore.Application.Queries;

public record ObterExtratoQuery(long NumeroConta) : IRequest<IEnumerable<Transacao>>;