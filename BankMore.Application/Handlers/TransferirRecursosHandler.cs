using BankMore.Application.Commands;
using BankMore.Domain.Interfaces;
using MediatR;

namespace BankMore.Application.Handlers;

public class EfetuarTransferenciaHandler : IRequestHandler<EfetuarTransferenciaCommand, bool>
{
    private readonly IContaCorrenteRepository _repository;

    public EfetuarTransferenciaHandler(IContaCorrenteRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(EfetuarTransferenciaCommand request, CancellationToken cancellationToken)
    {
        
        if (request.Valor <= 0)
            throw new ArgumentException("O valor da transferência deve ser maior que zero.");

        if (request.ContaOrigem == request.ContaDestino)
            throw new ArgumentException("A conta de destino deve ser diferente da conta de origem.");

        return await _repository.TransferirAsync(request.ContaOrigem, request.ContaDestino, request.Valor);
    }
}