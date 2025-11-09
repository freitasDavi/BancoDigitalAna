using BancoDigitalAna.BuildingBlocks.Domain;
using BancoDigitalAna.Conta.Domain.Entities;

namespace BancoDigitalAna.Conta.Domain.Repositories
{
    public interface IContaRepository : IRepository<ContaCorrente>
    {
        Task NovaConta(ContaCorrente contaCorrente);
        Task<ContaCorrente?> RecuperarPorCPF(string cpf);
        Task<ContaCorrente?> RecuperarPorNumeroConta(int numeroConta);
    }
}
