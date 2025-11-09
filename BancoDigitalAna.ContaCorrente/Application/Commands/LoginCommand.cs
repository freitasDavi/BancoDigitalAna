using BancoDigitalAna.BuildingBlocks.CQRS;

namespace BancoDigitalAna.Conta.Application.Commands
{
    public record LoginCommand (string NumeroOuCpf, string senha)
        : ICommand<LoginResponse>
    {
    }

    public record LoginResponse(string Token) { }
}
