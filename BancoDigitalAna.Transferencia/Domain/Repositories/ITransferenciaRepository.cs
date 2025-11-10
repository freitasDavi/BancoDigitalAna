using BancoDigitalAna.Transferencia.Domain.Entities;

namespace BancoDigitalAna.Transferencia.Domain.Repositories
{
    public interface ITransferenciaRepository
    {
        Task<Guid> InserirAsync(Transferencias transferencia);
        Task<Transferencias?> ObterPorIdRequisicaoAsync(Guid idRequisicao);
    }
}
