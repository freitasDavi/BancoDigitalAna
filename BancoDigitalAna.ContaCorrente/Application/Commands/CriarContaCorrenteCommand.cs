using BancoDigitalAna.BuildingBlocks.CQRS;

namespace BancoDigitalAna.Conta.Application.Commands
{
    public record CriarContaCorrenteCommand (string Cpf, string NomeTitular, string Senha) 
        : ICommand<CriarContaCorrenteResponse>;

    public record CriarContaCorrenteResponse (string NumeroConta)
    {

    }
}
