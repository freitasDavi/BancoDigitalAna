namespace BancoDigitalAna.Transferencia.Application.DTOs
{
    public record NovoMovimentoRequest(Guid IdConta, decimal Valor, char Tipo)
    {
    }
}
