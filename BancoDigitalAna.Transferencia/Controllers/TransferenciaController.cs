using BancoDigitalAna.Transferencia.Application.Commands;
using BancoDigitalAna.Transferencia.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BancoDigitalAna.Transferencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
