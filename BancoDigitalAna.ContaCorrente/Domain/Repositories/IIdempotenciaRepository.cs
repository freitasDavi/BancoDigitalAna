using BancoDigitalAna.Conta.Domain.Entities;

namespace BancoDigitalAna.Conta.Domain.Repositories
{
    public interface IIdempotenciaRepository
    {
        Task<string?> RecuperarIdempotencia(Guid chaveRequisicao);
        Task SalvarAsync(Idempotencia idempotencia);
    }
}
