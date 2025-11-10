using Asp.Versioning;
using BancoDigitalAna.Transferencia.Application.Commands;
using BancoDigitalAna.Transferencia.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BancoDigitalAna.Transferencia.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class TransferenciaController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<TransferenciaResponse>> NovaTransferencia ([FromBody] NovaTransferenciaCommand command) 
        {
            var response = await _mediator.Send(command);

            return Ok(response);
        }

    }
}
