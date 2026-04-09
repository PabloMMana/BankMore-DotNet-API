using BankMore.Application.Queries;
using BankMore.Domain.Entities;
using BankMore.Domain.Interfaces;
using MediatR;

public class ObterExtratoHandler : IRequestHandler<ObterExtratoQuery, IEnumerable<Transacao>>
{
    private readonly IContaCorrenteRepository _repository;

    public ObterExtratoHandler(IContaCorrenteRepository repository) => _repository = repository;

    public async Task<IEnumerable<Transacao>> Handle(ObterExtratoQuery request, CancellationToken cancellationToken)
    {
        return await _repository.ObterExtratoAsync(request.NumeroConta);
    }
}