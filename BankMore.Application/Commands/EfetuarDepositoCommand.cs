using MediatR;

namespace BankMore.Application.Commands;

public record EfetuarDepositoCommand(
    int NumeroConta,
    decimal Valor
) : IRequest<bool>;