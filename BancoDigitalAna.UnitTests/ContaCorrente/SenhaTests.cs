using BancoDigitalAna.BuildingBlocks.Domain.Exceptions;
using BancoDigitalAna.ContaCorrente.Domain.ValueObjects;
using FluentAssertions;

namespace BancoDigitalAna.UnitTests.ContaCorrente
{
    public class SenhaTests
    {
        [Fact]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("  ")]
        public void SenhaVazia_EmBranco_ComEspaco_DeveLancarExcecao (string senha)
        {
            // Act
            var act = () => Senha.Criar(senha);

            act.Should().Throw<DomainException>()
                .WithMessage("Senha deve ter no mínimo 6 caracteres")
                .Where(e => e.ErrorCode == "INVALID_PASSWORD");
        }
    }
}
