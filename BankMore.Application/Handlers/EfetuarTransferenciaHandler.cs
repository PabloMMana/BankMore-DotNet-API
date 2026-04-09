using MediatR;

public record EfetuarTransferenciaCommand(
    long ContaOrigem,
    long ContaDestino,
    decimal Valor
) : IRequest<bool>;