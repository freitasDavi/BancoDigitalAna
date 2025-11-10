namespace BancoDigitalAna.Conta.Application.Services
{
    public interface IIdempotenciaService
    {
        Task<T?> ExecutarComIdempotenciaAsync<T>(
            Guid chaveIdempotencia,
            object requisicao,
            Func<Task<T>> operacao);
    }
}
