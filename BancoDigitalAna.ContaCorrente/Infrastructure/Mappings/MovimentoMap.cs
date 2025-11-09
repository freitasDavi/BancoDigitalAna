using BancoDigitalAna.Conta.Domain.Entities;
using BancoDigitalAna.Conta.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BancoDigitalAna.Conta.Infrastructure.Mappings
{
    public class MovimentoMap : IEntityTypeConfiguration<Movimento>
    {
        public void Configure(EntityTypeBuilder<Movimento> builder)
        {
            builder.ToTable("MOVIMENTO");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasColumnName("IDMOVIMENTO")
                .HasColumnType("VARCHAR2(36)")
                .HasConversion(
                    guid => guid.ToString(),
                    str => new Guid(str))
                .IsRequired();

            builder.OwnsOne(m => m.TipoMovimento, tipoMovimento =>
            {
                tipoMovimento.Property(tp => tp.Codigo)
                    .HasColumnName("TIPOMOVIMENTO")
                    .HasMaxLength(1)
                    .IsRequired();
            });

            builder.Property(m => m.IdContaCorrente)
                .HasColumnName("IDCONTACORRENTE")
                .IsRequired();

            builder.Property(m => m.Valor)
                .HasColumnName("VALOR")
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(m => m.DataMovimento)
                .HasColumnName("DATAMOVIMENTO")
                .HasConversion(new DateToStringConverter())
                .IsRequired();
        }
    }
}
