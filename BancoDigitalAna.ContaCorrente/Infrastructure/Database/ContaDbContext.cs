using BancoDigitalAna.Conta.Domain.Entities;
using BancoDigitalAna.Conta.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace BancoDigitalAna.Conta.Infrastructure.Database
{
    public class ContaDbContext : DbContext
    {
        public ContaDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ContaCorrente> Contas { get; set; }
        public DbSet<Movimento> Movimentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContaCorrenteMap());
            modelBuilder.ApplyConfiguration(new MovimentoMap());
        }
    }
}
