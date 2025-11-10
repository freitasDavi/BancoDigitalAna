using BancoDigitalAna.BuildingBlocks.Infrastructure;
using BancoDigitalAna.Conta.Domain.Entities;
using BancoDigitalAna.Conta.Domain.Repositories;
using BancoDigitalAna.Conta.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BancoDigitalAna.Conta.Infrastructure.Repositories
{
    public class IdempotenciaRepository(ContaDbContext _context, IUnitOfWork _unitOfWork)
        : IIdempotenciaRepository
    {
        public async Task<string?> RecuperarIdempotencia(Guid chaveRequisicao)
        {
            var resultado = await _context.Idempotencias
                .Where(i => i.ChaveRequisicao == chaveRequisicao)
                .Select(i => i.Resultado)
                .FirstOrDefaultAsync();

            return resultado;
        }

        public async Task SalvarAsync(Idempotencia idempotencia)
        {
            await _context.AddAsync(idempotencia);

            await _unitOfWork.CommitAsync();
        }
    }
}
