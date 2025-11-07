using BancoDigitalAna.BuildingBlocks.Domain.Exceptions;
using BancoDigitalAna.Conta.Domain.ValueObjects;
using FluentAssertions;

namespace BancoDigitalAna.UnitTests.ContasCorrente
{

    public class CpfTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public void CpfVazio_EmBranco_ComEspaco_DeveLancarExcecao(string cpfTexto)
        {
            // Act
            var act = () => Cpf.Criar(cpfTexto);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("O campo CPF não pode ser vazio")
                .Where(e => e.ErrorCode == "INVALID_DOCUMENT") ;
        }

        [Theory]
        [InlineData("111.111.111-11")]
        [InlineData("000.000.000-00")]
        [InlineData("789.456.123-10")]
        public void CpfInvlaido_DeveLancarExcecao (string cpfTexto)
        {
            // Act
            var act = () => Cpf.Criar(cpfTexto);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("CPF Inválido")
                .Where(e => e.ErrorCode == "INVALID_DOCUMENT");
        }

        [Theory]
        [InlineData("123.456.789-09")]
        [InlineData("12345678909")]
        public void CpfValido_DeveRetornarSucesso(string  cpfTexto)
        {
            // Act
            var cpf = Cpf.Criar(cpfTexto);

            // Assert
            cpf.Should().NotBeNull();
            cpf.Numero.Should().Be("12345678909");
        }

    }
}
