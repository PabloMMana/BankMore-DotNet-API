using BankMore.Domain.Entities;
using BankMore.Domain.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BankMore.Infrastructure.Repositories
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=BankMoreDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public async Task<int> AdicionarAsync(ContaCorrente conta)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
               
                string sql = @"INSERT INTO contacorrente (nome, ativo, senha, salt, cpf) 
                       VALUES (@Nome, @Ativo, @Senha, @Salt, @Cpf);
                       SELECT CAST(SCOPE_IDENTITY() as int);";

                
                var idGerado = await db.ExecuteScalarAsync<int>(sql, new
                {
                    conta.Nome,
                    conta.Ativo,
                    conta.Senha,
                    conta.Salt,
                    conta.Cpf
                });

                return idGerado;
            }
        }

        public async Task<IEnumerable<ContaCorrente>> ListarAtivasAsync()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                
                string sql = "SELECT numero_conta as NumeroDaConta, nome, salt, ativo, cpf, saldo FROM contacorrente WHERE ativo = 1";
                return await db.QueryAsync<ContaCorrente>(sql);
            }
        }

        public async Task<bool> DesativarAsync(int numeroConta)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = "UPDATE contacorrente SET ativo = 0 WHERE numero_conta = @numeroConta";
                var linhasAfetadas = await db.ExecuteAsync(sql, new { numeroConta });
                return linhasAfetadas > 0;
            }
        }

        public async Task<ContaCorrente?> ObterParaLoginAsync(string login)
        {
            using (var db = new SqlConnection(_connectionString))
            {
              
                string sql = "SELECT *, numero_conta as NumeroDaConta FROM contacorrente WHERE cpf = @login OR numero_conta = @login";
                return await db.QueryFirstOrDefaultAsync<ContaCorrente>(sql, new { login });
            }
        }

        public async Task<ContaCorrente> ObterPorNumeroAsync(string numeroConta)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                string sql = "SELECT *, numero_conta as NumeroDaConta FROM contacorrente WHERE numero_conta = @numero";
                return await db.QueryFirstOrDefaultAsync<ContaCorrente>(sql, new { numero = numeroConta });
            }
        }

        public async Task AtualizarAsync(ContaCorrente conta)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                string sql = "UPDATE contacorrente SET saldo = @Saldo WHERE numero_conta = @NumeroDaConta";
                await db.ExecuteAsync(sql, conta);
            }
        }

        public async Task<bool> TransferirAsync(long numeroContaOrigem, long numeroContaDestino, decimal valor)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {                      
                        string sqlDebito = @"
                    UPDATE contacorrente 
                    SET saldo = saldo - @valor 
                    OUTPUT inserted.id
                    WHERE numero_conta = @numeroContaOrigem AND saldo >= @valor";

                        var idOrigem = await conn.QuerySingleOrDefaultAsync<int?>(sqlDebito,
                                       new { numeroContaOrigem, valor }, transaction);

                        if (idOrigem == null)
                            throw new Exception("Saldo insuficiente ou conta de origem não encontrada.");

                        string sqlCredito = @"
                    UPDATE contacorrente 
                    SET saldo = saldo + @valor 
                    OUTPUT inserted.id
                    WHERE numero_conta = @numeroContaDestino";

                        var idDestino = await conn.QuerySingleOrDefaultAsync<int?>(sqlCredito,
                                        new { numeroContaDestino, valor }, transaction);

                        if (idDestino == null)
                            throw new Exception("Conta de destino não encontrada.");

                        string sqlLog = @"
                    INSERT INTO transacao (conta_origem_id, conta_destino_id, valor, tipo, data_transacao) 
                    VALUES (@idOrigem, @idDestino, @valor, 'Transferencia', GETDATE())";

                        await conn.ExecuteAsync(sqlLog, new { idOrigem, idDestino, valor }, transaction);

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        
        }
        public async Task<bool> DepositarAsync(long numeroConta, decimal valor)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Primeiro, precisamos de obter o ID real da conta usando o número dela
                        // Também já aproveitamos para atualizar o saldo usando o numero_conta no WHERE
                        string sqlUpdate = @"
                    UPDATE contacorrente 
                    SET saldo = saldo + @valor 
                    OUTPUT inserted.id -- Isso retorna o ID da conta que foi alterada
                    WHERE numero_conta = @numeroConta";

                        var idReal = await conn.QuerySingleOrDefaultAsync<int?>(sqlUpdate,
                                     new { numeroConta, valor }, transaction);

                        if (idReal == null)
                            throw new Exception("Conta não encontrada com o número informado.");

                        // 2. Agora gravamos o log na tabela de transação usando o ID real (que a FK exige)
                        string sqlLog = @"
                    INSERT INTO transacao (conta_origem_id, conta_destino_id, valor, tipo, data_transacao) 
                    VALUES (@idReal, @idReal, @valor, 'Deposito', GETDATE())";

                        await conn.ExecuteAsync(sqlLog, new { idReal, valor }, transaction);

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public async Task<IEnumerable<Transacao>> ObterExtratoAsync(long numeroConta)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                // Buscamos as transações onde o número da conta aparece como origem ou destino
                string sql = @"
            SELECT t.* FROM transacao t
            INNER JOIN contacorrente c_origem ON t.conta_origem_id = c_origem.id
            INNER JOIN contacorrente c_destino ON t.conta_destino_id = c_destino.id
            WHERE c_origem.numero_conta = @numeroConta 
               OR c_destino.numero_conta = @numeroConta
            ORDER BY t.data_transacao DESC";

                return await conn.QueryAsync<Transacao>(sql, new { numeroConta });
            }
        }
    }
}