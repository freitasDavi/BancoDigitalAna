using Asp.Versioning;
using BancoDigitalAna.BuildingBlocks.Domain.Exceptions;
using BancoDigitalAna.BuildingBlocks.DTOs;
using BancoDigitalAna.BuildingBlocks.Infrastructure.Auth;
using BancoDigitalAna.Conta.Application.Commands;
using BancoDigitalAna.Conta.Application.DTOs;
using BancoDigitalAna.Conta.Application.Queries;
using BancoDigitalAna.Conta.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BancoDigitalAna.Conta.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ContaController(IMediator _mediator, ILoggedUser _loggedUser) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> NovaConta([FromBody] CriarContaCorrenteCommand command)
        {
            
            var response = await _mediator.Send(command);

            return Created($"{response.NumeroConta}", response);
            
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [Authorize]
        [HttpPatch("inativar")]
        public async Task<IActionResult> InativarConta([FromBody] InativarContaRequest request)
        {
            var contaId = Guid.Parse(_loggedUser.ContaId);

            await _mediator.Send(new InativarContaCommand(contaId, request.Senha));

            return NoContent();
        }

        [Authorize]
        [HttpPost("movimento")]
        public async Task<IActionResult> NovoMovimento([FromBody] NovoMovimentoRequest request)
        {
            var contaId = Guid.Parse(_loggedUser.ContaId);

            await _mediator.Send(new NovaMovimentacaoContaCorrenteCommand(request.NumeroConta, request.Valor, request.Tipo, contaId));

            return NoContent();
        }

        [Authorize]
        [HttpGet("consulta")]
        public async Task<ActionResult<ConsultaSaldoResponse>> ConsultaSaldo()
        {
            var contaId = Guid.Parse(_loggedUser.ContaId);

            var response = await _mediator.Send(new ConsultaSaldoQuery(contaId));

            return Ok(response);
        }

        [Authorize]
        [HttpGet("{numeroConta}")]
        public async Task<ActionResult<ContaCorrente>> ConsultaContaPorNumero([FromRoute] string numeroConta)
        {
            var response = await _mediator.Send(new ConsultaContaPorNumeroQuery(numeroConta));

            return Ok(response);
        }
    }
}
