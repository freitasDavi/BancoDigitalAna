using BancoDigitalAna.Conta.Domain.Entities;
using BancoDigitalAna.Conta.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BancoDigitalAna.Conta.Infrastructure.Database
{
    public class ContaDbContext : DbContext
    {
        public ContaDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ContaCorrente> Contas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContaCorrenteMap());
            //base.OnModelCreating(modelBuilder);
        }
    }
}
