using BancoDigitalAna.BuildingBlocks.Infrastructure;
using BancoDigitalAna.Conta.Domain.Entities;
using BancoDigitalAna.Conta.Domain.Repositories;
using BancoDigitalAna.Conta.Infrastructure.Database;

namespace BancoDigitalAna.Conta.Infrastructure.Repositories
{
    public class ContaRepository(ContaDbContext _context, IUnitOfWork _unitOfWork) : IContaRepository
    {
        public async Task NovaConta(ContaCorrente contaCorrente)
        {
            await _context.Contas.AddAsync(contaCorrente);

            await _unitOfWork.CommitAsync();
        }
    }
}
