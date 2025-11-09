using Asp.Versioning;
using BancoDigitalAna.BuildingBlocks.Domain.Exceptions;
using BancoDigitalAna.BuildingBlocks.DTOs;
using BancoDigitalAna.BuildingBlocks.Infrastructure.Auth;
using BancoDigitalAna.Conta.Application.Commands;
using BancoDigitalAna.Conta.Application.DTOs;
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
        public async Task<IActionResult> novaConta([FromBody] CriarContaCorrenteCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);

                return Created($"{response.NumeroConta}", response);
            }
            catch (DomainException ex)
            {
                return BadRequest(new BadRequestResponse(ex.Message, ex.ErrorCode));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> login([FromBody] LoginCommand command)
        {
            //try
            //{
                var response = await _mediator.Send(command);

                return Ok(response);
            //} catch (Exception ex)
            //{
            //    return BadRequest(ex.Message);
            //}
        }

        [Authorize]
        [HttpPatch("inativar")]
        public async Task<IActionResult> inativarConta([FromBody] InativarContaRequest request)
        {
            var contaId = Guid.Parse(_loggedUser.ContaId);

            await _mediator.Send(new InativarContaCommand(contaId, request.Senha));

            return NoContent();
        }
    }
}
