using BancoDigitalAna.BuildingBlocks.CQRS;
using BancoDigitalAna.BuildingBlocks.Infrastructure.Auth.Exceptions;
using BancoDigitalAna.Conta.Application.Commands;
using BancoDigitalAna.Conta.Domain.Entities;
using BancoDigitalAna.Conta.Domain.Repositories;
using MediatR;

namespace BancoDigitalAna.Conta.Application.Handlers
{
    public class NovaMovimentacaoNaContaCorrenteCommandHandler(IContaRepository _repository) : ICommandHandler<NovaMovimentacaoContaCorrenteCommand>
    {
        public async Task<Unit> Handle(NovaMovimentacaoContaCorrenteCommand request, CancellationToken cancellationToken)
        {
            ContaCorrente? conta;

            if (!string.IsNullOrEmpty(request.NumeroConta))
            {
                conta = await _repository.RecuperarPorNumeroConta(int.Parse(request.NumeroConta));
            } else
            {
                conta = await _repository.RecuperarPorId((Guid)request.IdConta);
            }

            if (conta == null)
                throw new UnauthorizedException("Usuario problematico");

            conta.AdicionarMovimento(request.Tipo, request.Valor);

            await _repository.AtualizarAsync(conta);

            return Unit.Value;
        }
    }
}
