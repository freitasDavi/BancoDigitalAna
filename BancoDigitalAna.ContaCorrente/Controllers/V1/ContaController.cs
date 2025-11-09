using Asp.Versioning;
using BancoDigitalAna.BuildingBlocks.Domain.Exceptions;
using BancoDigitalAna.BuildingBlocks.DTOs;
using BancoDigitalAna.Conta.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BancoDigitalAna.Conta.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ContaController(IMediator mediator) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> novaConta([FromBody] CriarContaCorrenteCommand command)
        {
            try
            {
                var response = await mediator.Send(command);

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
    }
}
