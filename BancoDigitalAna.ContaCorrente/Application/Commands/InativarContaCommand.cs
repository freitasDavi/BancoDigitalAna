using BancoDigitalAna.BuildingBlocks.CQRS;

namespace BancoDigitalAna.Conta.Application.Commands
{
    public record InativarContaCommand(Guid IdConta,string Senha) : ICommand
    {
    }
}
