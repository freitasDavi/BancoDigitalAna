using BancoDigitalAna.Conta.Domain.Entities;
using FluentAssertions;
using System.Text.RegularExpressions;

namespace BancoDigitalAna.UnitTests.ContasCorrente
{
    public class ContaCorrenteTests
    {
        [Theory]
        [InlineData("123.456.789-09", "Teste Junior", "Senh4!")]
        public void DadosValidos_DevemRetornarSucesso(string cpf, string nome, string senha)
        {
            // Act
            var contaCorrente = ContaCorrente.Criar(cpf, nome, senha);

            // Assert
            contaCorrente.NomeTitular.Should().BeEquivalentTo(nome);
            contaCorrente.Cpf.Numero.Should().BeEquivalentTo(Regex.Replace(cpf, @"[^\d]", ""));
        }
    }
}
