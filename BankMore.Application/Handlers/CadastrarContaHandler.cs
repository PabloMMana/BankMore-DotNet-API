using MediatR;
using BankMore.Domain.Entities;
using BankMore.Domain.Interfaces;      
using BankMore.Infrastructure.Services; 
using BankMore.Application.Commands;    
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BankMore.Application.Handlers
{
    public class CadastrarContaHandler : IRequestHandler<CadastrarContaCommand, int>,
                                         IRequestHandler<EfetuarLoginCommand, string>
    {
        private readonly IContaCorrenteRepository _repository;
        private readonly ISecurityService _securityService;
        private readonly TokenService _tokenService;

        
        public CadastrarContaHandler(
            IContaCorrenteRepository repository,
            ISecurityService securityService,
            TokenService tokenService)
        {
            _repository = repository;
            _securityService = securityService;
            _tokenService = tokenService;
        }

        public async Task<int> Handle(CadastrarContaCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Senha))
                throw new ArgumentException("A senha fornecida é nula ou inválida.");

            string salt = Guid.NewGuid().ToString();
            string senhaProtegida = _securityService.GerarHash(request.Senha, salt);

            var novaConta = new ContaCorrente
            {
                Nome = request.Nome,
                Ativo = 1,
                Cpf = request.Cpf,
                Salt = salt,
                Senha = senhaProtegida
                
            };

            return await _repository.AdicionarAsync(novaConta);
        }

        public async Task<string> Handle(EfetuarLoginCommand request, CancellationToken ct)
        {
            var conta = await _repository.ObterParaLoginAsync(request.CpfOuConta);

            if (conta == null || !_securityService.VerificarSenha(request.Senha, conta.Senha, conta.Salt))
            {
                throw new UnauthorizedAccessException("USER_UNAUTHORIZED");
            }

            return _tokenService.GerarToken(conta);
        }
    }
}