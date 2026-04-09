using BankMore.Application.Commands;
using BankMore.Domain.Interfaces;
using MediatR;

namespace BankMore.Application.Handlers;

public class EfetuarDepositoHandler : IRequestHandler<EfetuarDepositoCommand, bool>
{
    private readonly IContaCorrenteRepository _repository;

    public EfetuarDepositoHandler(IContaCorrenteRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(EfetuarDepositoCommand request, CancellationToken cancellationToken)
    {
        if (request.Valor <= 0)
            throw new ArgumentException("O valor do depósito deve ser maior que zero.");

        return await _repository.DepositarAsync(request.NumeroConta, request.Valor);
    }
}