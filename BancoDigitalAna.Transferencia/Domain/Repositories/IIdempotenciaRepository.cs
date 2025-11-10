namespace BancoDigitalAna.Transferencia.Domain.Repositories
{
    public interface IIdempotenciaRepository
    {
        Task<string?> RecuperarIdempotencia(string chaveIdempotencia);
        Task SalvarAsync(string chaveIdempotencia, string requisicao, string resultado);
    }
}
