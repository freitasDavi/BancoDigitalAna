using BancoDigitalAna.BuildingBlocks.CQRS;
using BancoDigitalAna.BuildingBlocks.Infrastructure.Auth.Exceptions;
using BancoDigitalAna.Conta.Application.Commands;
using BancoDigitalAna.Conta.Domain.Repositories;
using MediatR;

namespace BancoDigitalAna.Conta.Application.Handlers
{
    public class InativarContaCommandHandler(IContaRepository _contaRepository) : ICommandHandler<InativarContaCommand>
    {
        public async Task<Unit> Handle(InativarContaCommand request, CancellationToken cancellationToken)
        {
            var conta = await _contaRepository.RecuperarPorId(request.IdConta) ?? throw new UnauthorizedException("Conta não encontrada");
            
            conta.Inativar(request.Senha);

            await _contaRepository.InativarConta(conta);

            return Unit.Value;
        }
    }
}
