using BancoDigitalAna.BuildingBlocks.Domain.Exceptions;
using BancoDigitalAna.Transferencia.Domain.Entities;
using FluentAssertions;

namespace BancoDigitalAna.UnitTests.Transferencia
{
    public class TransferenciaTest
    {
        [Theory]
        [InlineData(-53)]
        [InlineData(0)]
        public void Transferencia_ComValorMenorQueZero_LancaExcecao(decimal valor)
        {
            // Arrange 
            var contaOrigem = Guid.NewGuid();
            var contaDestino = Guid.NewGuid();

            // Act
            var act = () => Transferencias.Criar(contaOrigem, contaDestino, valor);

            act.Should().Throw<DomainException>()
                .WithMessage("Valor da trasnferência deve ser maior que zero")
                .Where(ex => ex.ErrorCode == "INVALID_VALUE");
        }

        [Fact]
        public void Transferencia_Valida_DeveRetornarSucesso ()
        {
            // Arrange
            var contaOrigem = Guid.NewGuid();
            var contaDestino = Guid.NewGuid();
            decimal valor = 10;

            var transferencia = Transferencias.Criar(contaOrigem, contaDestino, valor);

            transferencia.Should().NotBeNull();
            transferencia.Valor.Should().Be(valor);
            transferencia.ContaOrigem.Should().Be(contaOrigem);
            transferencia.ContaDestino.Should().Be(contaDestino);
        }
    }
}
