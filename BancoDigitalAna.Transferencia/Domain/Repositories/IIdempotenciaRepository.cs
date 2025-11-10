namespace BancoDigitalAna.Transferencia.Domain.Repositories
{
    public interface IIdempotenciaRepository
    {
        Task<string?> RecuperarIdempotencia(Guid chaveIdempotencia);
        Task SalvarAsync(Guid chaveIdempotencia, string requisicao, string resultado);
    }
}
