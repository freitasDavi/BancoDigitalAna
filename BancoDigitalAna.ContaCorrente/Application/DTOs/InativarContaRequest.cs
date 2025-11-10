namespace BancoDigitalAna.Conta.Application.DTOs
{
    public record InativarContaRequest (string Senha, Guid ChaveIdempotencia)
    {
    }
}
