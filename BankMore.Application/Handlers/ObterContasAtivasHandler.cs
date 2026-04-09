using BankMore.Application.Queries;
using BankMore.Domain.Entities;
using BankMore.Domain.Interfaces;
using MediatR;

public class ObterContasAtivasHandler : IRequestHandler<ObterContasAtivasQuery, IEnumerable<ContaCorrente>>
{
    private readonly IContaCorrenteRepository _repository;

    public ObterContasAtivasHandler(IContaCorrenteRepository repository)
        => _repository = repository;

    public async Task<IEnumerable<ContaCorrente>> Handle(ObterContasAtivasQuery request, CancellationToken ct)
    {
        return await _repository.ListarAtivasAsync();
    }
}