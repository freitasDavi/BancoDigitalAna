using BancoDigitalAna.BuildingBlocks.CQRS;
using BancoDigitalAna.Transferencia.Application.DTOs;

namespace BancoDigitalAna.Transferencia.Application.Commands
{
    public record NovaTransferenciaCommand (Guid ContaOrigemId, string ContaDestinoNumero, decimal Valor, Guid ChaveIdempotencia) : ICommand<TransferenciaResponse>
    {
    }
}
