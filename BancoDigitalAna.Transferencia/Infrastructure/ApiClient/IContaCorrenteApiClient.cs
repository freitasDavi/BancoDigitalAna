using BancoDigitalAna.Transferencia.Application.DTOs;

namespace BancoDigitalAna.Transferencia.Infrastructure.ApiClient
{
    public interface IContaCorrenteApiClient
    {
        Task<bool> ValidarContaAsync(Guid contaId);
        Task RealizarMovimentoAsync(NovoMovimentoRequest request);
        Task<ConsultaContaPorNumeroResponse?> ConsultaContaPorNumero(string numeroConta);
    }
}
