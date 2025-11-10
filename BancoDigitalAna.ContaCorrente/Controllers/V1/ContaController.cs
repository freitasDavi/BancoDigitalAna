using Asp.Versioning;
using BancoDigitalAna.BuildingBlocks.Infrastructure;
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
    /// <summary>
    /// Contas Controller
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ContaController(IMediator _mediator, ILoggedUser _loggedUser) : ControllerBase
    {
        /// <summary>
        /// Crie uma nova conta
        /// </summary>
        /// <remarks>
        /// Exemplo:
        /// 
        /// post /api/v1/Conta
        /// {
        ///     "nomeTitular": "Nome Teste",
        ///     "senha": "senha1234",
        ///     "cpf": "123.456.789-09"
        /// }
        /// 
        /// </remarks>
        /// <param name="command">Dados da conta a ser aberta</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> NovaConta([FromBody] CriarContaCorrenteCommand command)
        {
            
            var response = await _mediator.Send(command);

            return Created($"{response.NumeroConta}", response);
            
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <remarks>
        /// Exemplo:
        /// 
        /// post /api/v1/Conta/login
        /// {
        ///     
        ///     "senha": "senha1234",
        ///     "numeroOuCpf": "123.456.789-09"
        /// }
        /// 
        /// </remarks>
        /// <param name="command">Dados do login</param>
        /// <returns>Token de acesso</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        /// <summary>
        /// Inativar
        /// </summary>
        /// <remarks>
        /// Exemplo:
        /// 
        /// patch /api/v1/Conta/inativar
        /// {
        ///     
        ///     "senha": "senha1234",
        ///     "chaveIdempotencia": "63130d5c-5404-4761-af42-124daffce258"
        /// }
        /// 
        /// </remarks>
        /// <param name="command">Dados para desativação de conta</param>
        [Authorize]
        [HttpPatch("inativar")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InativarConta([FromBody] InativarContaRequest request)
        {
            var contaId = Guid.Parse(_loggedUser.ContaId);

            await _mediator.Send(new InativarContaCommand(contaId, request.Senha, request.ChaveIdempotencia));

            return NoContent();
        }

        /// <summary>
        /// Novo movimento
        /// </summary>
        /// <remarks>
        /// Exemplo:
        /// 
        /// post /api/v1/Conta/movimento
        /// {
        ///     
        ///     "NumeroConta": "12345678",
        ///     "Valor": 345,
        ///     "tipo": 'D' // ou 'C'
        ///     "chaveIdempotencia": "63130d5c-5404-4761-af42-124daffce258"
        /// }
        /// 
        /// </remarks>
        /// <param name="command">Dados para nova movimentação</param>
        [Authorize]
        [HttpPost("movimento")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> NovoMovimento([FromBody] NovoMovimentoRequest request)
        {
            var contaId = Guid.Parse(_loggedUser.ContaId);

            await _mediator.Send(new NovaMovimentacaoContaCorrenteCommand(request.NumeroConta, request.Valor, request.Tipo, contaId, request.ChaveIdempotencia));

            return NoContent();
        }

        /// <summary>
        /// Consulta saldo do usuário logado
        /// </summary>
        /// <remarks>
        /// Exemplo:
        /// 
        /// get /api/v1/Conta/consulta
        /// {
        ///     
        /// }
        /// 
        /// </remarks>
        /// <returns>
        /// NumeroConta
        /// Saldo
        /// Titular
        /// DataHora
        /// </returns>
        [Authorize]
        [HttpGet("consulta")]
        [ProducesResponseType(typeof(ConsultaSaldoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<ConsultaSaldoResponse>> ConsultaSaldo()
        {
            var contaId = Guid.Parse(_loggedUser.ContaId);

            var response = await _mediator.Send(new ConsultaSaldoQuery(contaId));

            return Ok(response);
        }

        /// <summary>
        /// Consulta dados da conta corrente
        /// </summary>
        /// <remarks>
        /// Exemplo:
        /// 
        /// get /api/v1/Conta/:numeroConta
        /// {
        ///     
        /// }
        /// 
        /// </remarks>
        /// <returns>
        /// NumeroConta
        /// CPF
        /// IdConta
        /// Titular
        /// </returns>
        [Authorize]
        [HttpGet("{numeroConta}")]
        [ProducesResponseType(typeof(ContaCorrente), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<ActionResult<ContaCorrente>> ConsultaContaPorNumero([FromRoute] string numeroConta)
        {
            var response = await _mediator.Send(new ConsultaContaPorNumeroQuery(numeroConta));

            return Ok(response);
        }
    }
}
