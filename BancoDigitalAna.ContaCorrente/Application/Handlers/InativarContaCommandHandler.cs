using BancoDigitalAna.BuildingBlocks.CQRS;
using BancoDigitalAna.BuildingBlocks.Infrastructure.Auth.Exceptions;
using BancoDigitalAna.Conta.Application.Commands;
using BancoDigitalAna.Conta.Application.Services;
using BancoDigitalAna.Conta.Domain.Repositories;
using MediatR;

namespace BancoDigitalAna.Conta.Application.Handlers
{
    public class InativarContaCommandHandler(IContaRepository _contaRepository, IIdempotenciaService idempotenciaService) : ICommandHandler<InativarContaCommand>
    {
        public async Task<Unit> Handle(InativarContaCommand request, CancellationToken cancellationToken)
        {
            return await idempotenciaService.ExecutarComIdempotenciaAsync(request.ChaveIdempotencia, new
            {
                request.IdConta,
                request.Senha
            }, async () =>
            {
                var conta = await _contaRepository.RecuperarPorId(request.IdConta) ?? throw new UnauthorizedException("Conta não encontrada");

                conta.Inativar(request.Senha);

                await _contaRepository.InativarConta(conta);

                return Unit.Value;
            });
        }
    }
}
