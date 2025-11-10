using BancoDigitalAna.BuildingBlocks.CQRS;
using BancoDigitalAna.Conta.Application.DTOs;

namespace BancoDigitalAna.Conta.Application.Queries
{
    public record ConsultaSaldoQuery(Guid codigoConta) : IQuery<ConsultaSaldoResponse>
    {
    }
}
