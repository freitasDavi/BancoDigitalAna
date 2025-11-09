using BancoDigitalAna.Conta.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BancoDigitalAna.Conta.Infrastructure.Mappings
{
    public class ContaCorrenteMap : IEntityTypeConfiguration<ContaCorrente>
    {
        public void Configure(EntityTypeBuilder<ContaCorrente> builder)
        {
            builder.ToTable("CONTACORRENTE");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .HasColumnName("IDCONTACORRENTE")
                .HasColumnType("VARCHAR2(36)")
                .HasConversion(
                    guid => guid.ToString(),
                    str => new Guid(str))
                .IsRequired();

            builder.Property(c => c.NumeroConta)
                .HasColumnName("NUMERO")
                .HasMaxLength(8)
                .IsRequired();

            builder.HasIndex(c => c.NumeroConta)
                .IsUnique();

            builder.Property(c => c.NomeTitular)
                .HasColumnName("NOME")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(c => c.Ativo)
                .HasColumnName("ATIVO")
                .HasColumnType("NUMBER(1)")
                .HasConversion(
                    v => v ? 1 : 0,
                    v => v == 1
                )
                .IsRequired();

            builder.OwnsOne(c => c.Cpf, cpf =>
            {
                cpf.Property(v => v.Numero)
                .HasColumnName("CPF")
                .HasMaxLength(11)
                .IsRequired();

                cpf.HasIndex(v => v.Numero)
                .IsUnique();
            });

            builder.OwnsOne(c => c.Senha, senha =>
            {
                senha.Property(s => s.Hash)
                .HasColumnName("SENHA")
                .HasMaxLength(255)
                .IsRequired();

                senha.Property(s => s.Salt)
                .HasColumnName("SALT")
                .HasMaxLength(255)
                .IsRequired();
            });
        }
    }
}
