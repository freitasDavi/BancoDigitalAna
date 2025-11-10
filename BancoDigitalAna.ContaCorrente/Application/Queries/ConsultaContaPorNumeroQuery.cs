using BancoDigitalAna.BuildingBlocks.CQRS;

namespace BancoDigitalAna.Conta.Application.Queries
{
    public record ConsultaContaPorNumeroQuery(string NumeroConta) : IQuery<ConsultaContaPorNumeroResponse>
    {
    }

    public record ConsultaContaPorNumeroResponse (Guid IdConta, string NomeTitular, string Cpf) { }
}
