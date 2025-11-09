using BancoDigitalAna.BuildingBlocks.Infrastructure;

namespace BancoDigitalAna.Conta.Infrastructure.Database
{
    public class UnitOfWork(ContaDbContext _context): IUnitOfWork
    {
        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
