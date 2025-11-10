using BancoDigitalAna.BuildingBlocks.CQRS;
using BancoDigitalAna.Conta.Application.Commands;
using BancoDigitalAna.Conta.Application.Services;
using BancoDigitalAna.Conta.Domain.Entities;
using BancoDigitalAna.Conta.Domain.Repositories;
using BancoDigitalAna.Conta.Infrastructure.Exceptions;
using System.Text.RegularExpressions;

namespace BancoDigitalAna.Conta.Application.Handlers
{
    public class CriarContaCorrenteCommandHandler(IContaRepository _repository, IIdempotenciaService _idempotenciaService) : ICommandHandler<CriarContaCorrenteCommand, CriarContaCorrenteResponse>
    {
        public async Task<CriarContaCorrenteResponse> Handle(CriarContaCorrenteCommand request, CancellationToken cancellationToken)
        {
            return await _idempotenciaService.ExecutarComIdempotenciaAsync(
                request.ChaveIdempotencia,
                new
                {
                    request.Cpf,
                    request.NomeTitular,
                    request.Senha,
                },
                async () =>
                {
                    var contaExistente = await _repository.RecuperarPorCPF(Regex.Replace(request.Cpf, @"[^\d]", ""));

                    if (contaExistente is not null)
                        throw new ContaCorrenteException("Por favor, informe um documento CPF válido ou que não esteja sendo utilizado", "INVALID_DOCUMENT");

                    var contaCorrente = ContaCorrente.Criar(
                        request.Cpf,
                        request.NomeTitular,
                        request.Senha
                        );

                    await _repository.NovaConta(contaCorrente);

                    return new CriarContaCorrenteResponse(contaCorrente.NumeroConta.ToString());
                });
        }
    }
}
