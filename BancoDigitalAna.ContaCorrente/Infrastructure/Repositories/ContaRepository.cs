using BancoDigitalAna.BuildingBlocks.Infrastructure;
using BancoDigitalAna.Conta.Domain.Entities;
using BancoDigitalAna.Conta.Domain.Repositories;
using BancoDigitalAna.Conta.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BancoDigitalAna.Conta.Infrastructure.Repositories
{
    public class ContaRepository(ContaDbContext _context, IUnitOfWork _unitOfWork) : IContaRepository
    {
        public async Task NovaConta(ContaCorrente contaCorrente)
        {
            await _context.Contas.AddAsync(contaCorrente);

            await _unitOfWork.CommitAsync();
        }

        public async Task<ContaCorrente?> RecuperarPorCPF(string cpf)
        {
            return await _context.Contas.Where(c => c.Cpf.Numero == cpf).FirstOrDefaultAsync();
        }

        public async Task<ContaCorrente?> RecuperarPorNumeroConta(int numeroConta)
        {
            return await _context.Contas.Where(c => c.NumeroConta == numeroConta).FirstOrDefaultAsync();
        }
    }
}
