using BancoDigitalAna.BuildingBlocks.CQRS;
using BancoDigitalAna.Conta.Application.Queries;
using BancoDigitalAna.Conta.Domain.Repositories;
using BancoDigitalAna.Conta.Infrastructure.Exceptions;

namespace BancoDigitalAna.Conta.Application.Handlers
{
    public class ConsultaContaPorNumeroQueryHandler(IContaRepository _repository) : IQueryHandler<ConsultaContaPorNumeroQuery, ConsultaContaPorNumeroResponse>
    {
        public async Task<ConsultaContaPorNumeroResponse> Handle(ConsultaContaPorNumeroQuery request, CancellationToken cancellationToken)
        {
            var conta = await _repository.RecuperarPorNumeroConta(int.Parse(request.NumeroConta));

            if (conta is null)
                throw new ContaCorrenteException($"Conta com o número {request.NumeroConta} não encontrada", "INVALID_ACCOUNT");

            conta.ContaEstaAtiva();

            return new ConsultaContaPorNumeroResponse(conta.Id, conta.NomeTitular, conta.Cpf.Numero);
        }
    }
}
