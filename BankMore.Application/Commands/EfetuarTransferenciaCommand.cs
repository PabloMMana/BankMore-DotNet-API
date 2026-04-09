using MediatR;

namespace BankMore.Application.Commands;

public record EfetuarTransferenciaCommand(
    long ContaOrigemId,
    long ContaDestinoId,
    decimal Valor
) : IRequest<bool>;