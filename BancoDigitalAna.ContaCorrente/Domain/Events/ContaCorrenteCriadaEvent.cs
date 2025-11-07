
using BancoDigitalAna.BuildingBlocks.Domain.Common;

namespace BancoDigitalAna.Conta.Domain.Events
{
    public record ContaCorrenteCriadaEvent
        (
            Guid ContaId,
            int NumeroConta,
            string Cpf
        ) : IDomainEvent
    {
        public DateTime DataDoEvento { get; } = DateTime.UtcNow;
    }
}
