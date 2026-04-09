using BankMore.Application.Commands;
using BankMore.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankMore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

     
        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [Tags("1. Login")]
        public async Task<IActionResult> Login([FromBody] EfetuarLoginCommand command)
        {
            try
            {
                
                var token = await _mediator.Send(command);
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Usuário ou senha inválidos" });
            }
        }


        [HttpPost]
        [Tags("2. Conta Corrente")]
        public async Task<IActionResult> Cadastrar([FromBody] CadastrarContaCommand command)
        {
            try
                    {
                var id = await _mediator.Send(command);
                return Ok(id); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro: {ex.Message} | {ex.InnerException?.Message}");
            }
        }

        [HttpPost("depositar")]
        [Tags("3. Depósito")]
        public async Task<IActionResult> Depositar([FromBody] EfetuarDepositoCommand command)
        {
            try
            {
                var sucesso = await _mediator.Send(command);
                if (sucesso)
                    return Ok(new { message = "Depósito realizado com sucesso!" });

                return BadRequest("Não foi possível realizar o depósito.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("ativas")]
        [Tags("4. Lista de Ativas")]
        public async Task<IActionResult> ListarAtivas()
        {
           
            var contas = await _mediator.Send(new ObterContasAtivasQuery());

            return Ok(contas);
        }

        [HttpPatch("{numeroConta}/desativar")]
        [Tags("5. Desativar")]
        public async Task<IActionResult> Desativar(int numeroConta)
        {
            var resultado = await _mediator.Send(new DesativarContaCommand { NumeroConta = numeroConta });

            if (!resultado)
                return NotFound("Conta não encontrada.");

            return Ok("Conta desativada com sucesso.");
        }

        [HttpPost("transferir")]
        [Tags("6. Transferir")]
        public async Task<IActionResult> Transferir([FromBody] EfetuarTransferenciaCommand command)
        {
            try
            {
                var sucesso = await _mediator.Send(command);
                if (sucesso)
                    return Ok(new { message = "Transferência realizada com sucesso!" });

                return BadRequest("Não foi possível realizar a transferência.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{numeroConta}/extrato")]
        [Tags("7. Extrato")]
        public async Task<IActionResult> GetExtrato(long numeroConta)
        {
            try
            {
                var extrato = await _mediator.Send(new ObterExtratoQuery(numeroConta));
                return Ok(extrato);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}