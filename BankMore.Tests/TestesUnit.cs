using BankMore.Application.Commands;
using BankMore.Application.Handlers;
using BankMore.Domain.Entities;
using BankMore.Domain.Interfaces;
using BankMore.Infrastructure.Services;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BankMore.Tests
{
    public class TestesUnit
    {
        private readonly Mock<IContaCorrenteRepository> _repositoryMock;
        private readonly Mock<ISecurityService> _securityServiceMock; // Novo Mock
        private readonly Mock<TokenService> _tokenServiceMock;      // Novo Mock
        private readonly CadastrarContaHandler _handler;

        public TestesUnit()
        {
            _repositoryMock = new Mock<IContaCorrenteRepository>();
            _securityServiceMock = new Mock<ISecurityService>();            
            _tokenServiceMock = new Mock<TokenService>(null);

            _handler = new CadastrarContaHandler(
                _repositoryMock.Object,
                _securityServiceMock.Object,
                _tokenServiceMock.Object); // Agora o Handler recebe tudo o que precisa
        }

        [Fact]
        public async Task CriarConta_DeveRetornarNumeroDaConta_QuandoDadosForemValidos()
        {
            
            var comando = new CadastrarContaCommand("Usuario Teste", "12345678901", "SenhaForte123");
            

            
            _repositoryMock.Setup(repo => repo.AdicionarAsync(It.IsAny<ContaCorrente>()))
                           .ReturnsAsync(1001);

           
            var resultado = await _handler.Handle(comando, CancellationToken.None);

           
            Assert.Equal(1001, resultado); 

           
            _repositoryMock.Verify(repo => repo.AdicionarAsync(It.IsAny<ContaCorrente>()), Times.Once);
        }
    }
}