using BancoDigitalAna.Conta.Domain.Entities;

namespace BancoDigitalAna.Conta.Domain.Repositories
{
    public interface IMovimentoRepository
    {
        Task novoMovimento(Movimento request);
    }
}
