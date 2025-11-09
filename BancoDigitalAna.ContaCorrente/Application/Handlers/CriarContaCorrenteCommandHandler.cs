using BancoDigitalAna.BuildingBlocks.CQRS;
using BancoDigitalAna.Conta.Application.Commands;
using BancoDigitalAna.Conta.Domain.Entities;
using BancoDigitalAna.Conta.Domain.Repositories;

namespace BancoDigitalAna.Conta.Application.Handlers
{
    public class CriarContaCorrenteCommandHandler(IContaRepository _repository) : ICommandHandler<CriarContaCorrenteCommand, CriarContaCorrenteResponse>
    {
        public async Task<CriarContaCorrenteResponse> Handle(CriarContaCorrenteCommand request, CancellationToken cancellationToken)
        {
            var contaCorrente = ContaCorrente.Criar(
                request.Cpf,
                request.NomeTitular,
                request.Senha
                );

            await _repository.NovaConta(contaCorrente);

            return new CriarContaCorrenteResponse(contaCorrente.NumeroConta.ToString());
        }
    }
}
