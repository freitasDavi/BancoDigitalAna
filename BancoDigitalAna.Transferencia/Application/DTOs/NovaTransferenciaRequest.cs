namespace BancoDigitalAna.Transferencia.Application.DTOs
{
    public record NovaTransferenciaRequest (string ChaveIdempotencia, Guid ContaDestinoNumero, decimal Valor)
    {
    }
}
