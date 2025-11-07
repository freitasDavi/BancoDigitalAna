using BancoDigitalAna.BuildingBlocks.Domain.Exceptions;
using BancoDigitalAna.ContaCorrente.Domain.ValueObjects;
using FluentAssertions;

namespace BancoDigitalAna.UnitTests.ContaCorrente
{
    public class SenhaTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("  ")]
        public void SenhaVazia_EmBranco_ComEspaco_DeveLancarExcecao (string senha)
        {
            // Act
            var act = () => Senha.Criar(senha);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Senha deve ter no mínimo 6 caractéres")
                .Where(e => e.ErrorCode == "INVALID_PASSWORD");
        }

        [Theory]
        [InlineData("senh4")]
        public void SenhaInvalida_MenosDe6Caracteres_DeveLancarExcecao(string senha)
        {
            // Act
            var act = () => Senha.Criar(senha);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage("Senha deve ter no mínimo 6 caractéres")
                .Where(e => e.ErrorCode == "INVALID_PASSWORD");
        }
    }
}
