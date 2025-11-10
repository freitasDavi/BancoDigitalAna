using Asp.Versioning;
using BancoDigitalAna.Transferencia.Application.Commands;
using BancoDigitalAna.Transferencia.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BancoDigitalAna.Transferencia.Controllers.V1
{
    /// <summary>
    /// Transferencias Controller
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class TransferenciaController(IMediator _mediator) : ControllerBase
    {
        /// <summary>
        /// Crie uma nova transferência
        /// </summary>
        /// <remarks>
        /// Exemplo:
        /// 
        /// post /api/v1/Conta
        /// {
        ///     "ContaOrigemId": "63130d5c-5404-4761-af42-124daffce258",
        ///     "ContaDestinoNumero": "12345678",
        ///     "ChaveIdempotencia": "63130d5c-5404-4761-af42-124daffce258"
        ///     "Valor": 345 
        /// }
        /// 
        /// </remarks>
        /// <param name="command">Dados da nova transferência</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<TransferenciaResponse>> NovaTransferencia ([FromBody] NovaTransferenciaCommand command) 
        {
            var response = await _mediator.Send(command);

            return Ok(response);
        }

    }
}
