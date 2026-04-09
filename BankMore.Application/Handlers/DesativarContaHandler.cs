using BankMore.Application.Commands;
using BankMore.Domain.Interfaces;
using MediatR;

public class DesativarContaHandler : IRequestHandler<DesativarContaCommand, bool>
{
    private readonly IContaCorrenteRepository _repository;

    public DesativarContaHandler(IContaCorrenteRepository repository)
        => _repository = repository;

    public async Task<bool> Handle(DesativarContaCommand request, CancellationToken ct)
    {
        return await _repository.DesativarAsync(request.NumeroConta);
    }
}