using BancoDigitalAna.BuildingBlocks.CQRS;
using BancoDigitalAna.Conta.Application.DTOs;
using BancoDigitalAna.Conta.Application.Queries;
using BancoDigitalAna.Conta.Domain.Repositories;

namespace BancoDigitalAna.Conta.Application.Handlers
{
    public class ConsultaSaldoQueryHandler(IContaRepository _repository) : IQueryHandler<ConsultaSaldoQuery, ConsultaSaldoResponse>
    {
        public async Task<ConsultaSaldoResponse> Handle(ConsultaSaldoQuery request, CancellationToken cancellationToken)
        {
            var conta = await _repository.RecuperarPorId(request.codigoConta);

            if (conta == null)
                throw new UnauthorizedAccessException("Apenas contas cadastradas podem consultar seu saldo");

            var saldoAtual = conta.ConsultarSaldo();

            return new ConsultaSaldoResponse(conta.NumeroConta.ToString(), conta.NomeTitular, DateTime.UtcNow, saldoAtual);
        }
    }
}
