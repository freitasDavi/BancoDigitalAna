using BancoDigitalAna.BuildingBlocks.CQRS;
using BancoDigitalAna.Conta.Domain.Entities;

namespace BancoDigitalAna.Conta.Application.Commands
{
    public record NovaMovimentacaoContaCorrenteCommand(string? NumeroConta, decimal Valor, char Tipo, Guid IdConta) : ICommand
    {
    }
}
