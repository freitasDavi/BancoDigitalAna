using BancoDigitalAna.Conta.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BancoDigitalAna.Conta.Infrastructure.Mappings
{
    public class IdempotenciaMap : IEntityTypeConfiguration<Idempotencia>
    {
        public void Configure(EntityTypeBuilder<Idempotencia> builder)
        {
            builder.ToTable("IDEMPOTENCIA");

            builder.HasKey(i => i.ChaveRequisicao);

            builder.Property(i => i.ChaveRequisicao)
                .HasColumnName("CHAVE_IDEMPOTENCIA")
                .HasColumnType("VARCHAR2(36)")
                .HasConversion(
                    guid => guid.ToString(),
                    str => new Guid(str))
                .IsRequired();

            builder.Property(i => i.Requisicao)
                .HasColumnName("REQUISICAO");

            builder.Property(i => i.Resultado)
                .HasColumnName("RESULTADO");

        }
    }
}
