using BancoDigitalAna.BuildingBlocks.Infrastructure;
using BancoDigitalAna.Conta.Domain.Entities;
using BancoDigitalAna.Conta.Domain.Repositories;
using BancoDigitalAna.Conta.Infrastructure.Database;

namespace BancoDigitalAna.Conta.Infrastructure.Repositories
{
    public class MovimentoRepository(ContaDbContext _context, IUnitOfWork _unitOfWork) : IMovimentoRepository
    {
        public async Task novoMovimento(Movimento request)
        {
            await _context.Movimentos.AddAsync(request);

            await _unitOfWork.CommitAsync();
        }
    }
}
