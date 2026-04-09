using BankMore.Domain.Entities;


namespace BankMore.Domain.Interfaces
{
    public interface IContaCorrenteRepository
    {
        Task<int> AdicionarAsync(ContaCorrente conta);
        Task<IEnumerable<ContaCorrente>> ListarAtivasAsync();
        Task<bool> DesativarAsync(int numeroConta);
        Task<ContaCorrente> ObterParaLoginAsync(string cpfOuConta);
        Task<ContaCorrente> ObterPorNumeroAsync(string numeroConta);
        Task AtualizarAsync(ContaCorrente conta);
        Task<bool> TransferirAsync(long origemId, long destinoId, decimal valor);
        Task<bool> DepositarAsync(long contaId, decimal valor);
        Task<IEnumerable<Transacao>> ObterExtratoAsync(long numeroConta);
    }
}