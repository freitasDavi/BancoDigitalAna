using BancoDigitalAna.BuildingBlocks.CQRS;
using BancoDigitalAna.BuildingBlocks.Infrastructure.Auth;
using BancoDigitalAna.BuildingBlocks.Infrastructure.Auth.Exceptions;
using BancoDigitalAna.Conta.Application.Commands;
using BancoDigitalAna.Conta.Domain.Entities;
using BancoDigitalAna.Conta.Domain.Repositories;

namespace BancoDigitalAna.Conta.Application.Handlers
{
    public class LoginCommandHandler(IContaRepository _repository, IJwtService _jwtService) :
        ICommandHandler<LoginCommand, LoginResponse>
    {
        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            ContaCorrente? conta;

            if (request.NumeroOuCpf != null && request.NumeroOuCpf.Length == 11)
            {
                var cpfLimpo = request.NumeroOuCpf.Replace(".", "").Replace("-", "");
                conta = await _repository.RecuperarPorCPF(cpfLimpo);    
            } else
            {
                conta = await _repository.RecuperarPorNumeroConta(int.Parse(request.NumeroOuCpf));
            }

            if (conta == null)
                throw new UnauthorizedException("Usuário ou senha inválidos");

            if (!conta.Senha.Validar(request.senha))
                throw new UnauthorizedException("Usuário ou senha inválidos");

            var token = _jwtService.GenerateToken(conta.Id.ToString(), conta.NumeroConta.ToString(), conta.Cpf);

            return new LoginResponse(token);
        }
    }
}
